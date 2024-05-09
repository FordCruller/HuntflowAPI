using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI.Data.Huntflow
{
    public class ApplicantStep
    {
        public int id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public DateTime? removed { get; set; }
        public int order { get; set; }
        public int? stayDuration { get; set; }

        public bool stepByStep { get; set; }

        private static ManualSettings settings = ManualSettings.GetInstance();

        public ApplicantStep(HuntflowRecruitmentStatuses.Status status)
        {
            this.id = status.id;
            this.type = status.type;
            this.name = status.name;
            this.removed = status.removed;
            this.order = status.order;
            this.stayDuration = status.stay_duration;

            
            if (settings.step_by_step.Contains(status.id))
            {
                stepByStep = true;
            }
        }

        public static bool MaintainsSequence(int stepIdA, int stepIdB)
        {
            var stepArray = settings.step_by_step;
            for (int i = 0; i < stepArray.Length; i++)
            {
                if (stepIdB == stepArray[i])
                {
                    stepIdB = i;
                    break;
                }
                if (stepIdA == stepArray[i])
                {
                    stepIdA = i;
                }
            }

            if(stepIdA < stepIdB)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ApplicantStep[] Convert(HuntflowRecruitmentStatuses statuses)
        {
            return HuntflowData.Converter.ConvertArray<ApplicantStep>(statuses.items);
        }

        public static Dictionary<int, ApplicantStep> GetDictionary(ApplicantStep[] steps)
        {
            var result = new Dictionary<int, ApplicantStep>();
            foreach (var step in steps)
            {
                result[step.id] = step;
            }
            return result;
        }
    }
}
