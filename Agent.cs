//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lapitskaya_GlazkiSave
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public partial class Agent
    {
        public int ID { get; set; }
        public int AgentTypeID { get; set; }

        //private string _agentTypeText;
        public string AgentTypeText
        {
            get
            {
                //if (_agentTypeText != null)
                //return _agentTypeText;

                var agentType = Lapitskaya_GlazkiSaveEntities.GetContext().AgentType.FirstOrDefault(p => p.ID == AgentTypeID);
                return agentType.Title;
            }
            set
            {

            }
}

        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public int Priority { get; set; }
        public string DirectorName { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
    }
}
