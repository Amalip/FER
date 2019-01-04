using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FER
{
    class FaceAPI
    {
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("cb15a1265db14cb1ae1d83393524911a", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");

        public async Task<Face[]> UploadAndDetectFaces_(Image image)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                image.Save(memStream, ImageFormat.Jpeg);
                long m = memStream.Length;

                IEnumerable<FaceAttributeType> faceAttributes = new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Emotion };

                var faces = await faceServiceClient.DetectAsync(memStream, returnFaceAttributes: faceAttributes);
                faces = faces.ToArray();

                foreach (var face in faces)
                {
                    FaceDescription(face);
                }

                return faces;
            }
            catch (Exception ex)
            {
                return new Face[0];
            }
        }

        private void FaceDescription(Face face)
        {
            /*using (var conexion = new SqlConnection(CadenaConexion))
            {
                var parametros = new DynamicParameters();
                int edad = 0, genero = 0;
                string emocion = "";

                genero = face.FaceAttributes.Gender != "female" ? 0 : 1;
                edad = (int)face.FaceAttributes.Age;
                //Emotions
                EmotionScores emotionScores = face.FaceAttributes.Emotion;
                string[] vals = new string[] { "Anger", "Contempt", "Disgust", "Fear", "Happiness", "Neutral", "Sadness", "Surprise" };
                double[] numbers = new double[] { emotionScores.Anger, emotionScores.Contempt, emotionScores.Disgust, emotionScores.Fear,
                    emotionScores.Happiness, emotionScores.Neutral, emotionScores.Sadness, emotionScores.Surprise };
                double maxVal = numbers.Max();

                for (int i = 0; i < numbers.Length; i++)
                    if (numbers[i] == maxVal)
                        emocion = vals[i];

                parametros.Add("edad", edad);
                parametros.Add("emocion", emocion);
                parametros.Add("genero", genero);

                conexion.Execute("sp_add", parametros, commandType: CommandType.StoredProcedure);
            }
            */

            int edad = 0, genero = 0;
            string emocion = "";

            genero = face.FaceAttributes.Gender != "female" ? 0 : 1;
            edad = (int)face.FaceAttributes.Age;
            //Emotions
            EmotionScores emotionScores = face.FaceAttributes.Emotion;
            string[] vals = new string[] { "Anger", "Contempt", "Disgust", "Fear", "Happiness", "Neutral", "Sadness", "Surprise" };
            double[] numbers = new double[] { emotionScores.Anger, emotionScores.Contempt, emotionScores.Disgust, emotionScores.Fear,
                    emotionScores.Happiness, emotionScores.Neutral, emotionScores.Sadness, emotionScores.Surprise };
        }

    }
}
