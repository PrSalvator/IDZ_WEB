//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDZ.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class APPOINTMENT_INFO
    {
        public string Пациент { get; set; }
        public string Доктор { get; set; }
        public string Специальность { get; set; }
        public System.DateTime Дата_приема { get; set; }
        public string Пришел_от { get; set; }
    }
}
