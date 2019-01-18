using Microsoft.ML.Data;
using System;

namespace CentennialTalk.Models.QuestionModels
{
    public class ResponseTrainingModel
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

    public class ClusteredResponses
    {
        public uint PredictedClusterId;
        public Guid QuestionId;
        public int ResponseId;
        public string ResponseContent;
        public Guid MemberId;
    }
}