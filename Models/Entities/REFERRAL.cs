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
    
    public partial class REFERRAL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REFERRAL()
        {
            this.APPOINTMENT1 = new HashSet<APPOINTMENT>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid APPOINTMENT_ID { get; set; }
        public Nullable<System.Guid> SPECIALIST_ID { get; set; }
        public Nullable<System.Guid> PROCEDURE_ID { get; set; }
    
        public virtual APPOINTMENT APPOINTMENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOINTMENT> APPOINTMENT1 { get; set; }
        public virtual PROCEDURE PROCEDURE { get; set; }
        public virtual SPECIALITY SPECIALITY { get; set; }
    }
}
