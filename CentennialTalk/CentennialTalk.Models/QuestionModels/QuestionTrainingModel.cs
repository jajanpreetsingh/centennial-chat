using Microsoft.ML.Data;

namespace CentennialTalk.Models.QuestionModels
{
    public class QuestionTrainingModel
    {
        [Column("0")]
        public string Question;

        [Column("1")]
        public string Answer;
    }

    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}