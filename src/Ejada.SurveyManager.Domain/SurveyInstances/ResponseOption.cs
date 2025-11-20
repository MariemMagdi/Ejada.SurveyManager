using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ejada.SurveyManager.SurveyInstances
{
    public class ResponseOption:FullAuditedEntity<Guid>
    {
        public Guid OptionId { get; private set; }
        public Guid ResponseId { get; private set; }

        private ResponseOption() { }

        private ResponseOption(Guid id , Guid optionId, Guid responseId)
            : base(id) 
        {
            SetOption(optionId);
            SetResponse(responseId);
        }

        public static ResponseOption Create(Guid id, Guid optionId, Guid responseId)
        {
            return new ResponseOption(id, optionId, responseId);
        }

        public ResponseOption SetOption(Guid optionId) 
        {
            if (optionId == Guid.Empty)
            {
                throw new BusinessException("ResponseOption.OptionId.Invalid")
                    .WithData("OptionId", optionId);
            }

            OptionId = optionId;
            return this;
        }
        public ResponseOption SetResponse(Guid responseId) 
        {
            if (responseId == Guid.Empty)
            {
                throw new BusinessException("ResponseOption.ResponseId.Invalid")
                    .WithData("ResponseId", responseId);
            }

            ResponseId = responseId;
            return this;
        }
    }
}
