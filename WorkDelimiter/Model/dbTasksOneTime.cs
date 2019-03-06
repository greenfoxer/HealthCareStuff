﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
// 
// Этот исходный код был создан с помощью xsd, версия=4.0.30319.33440.
// 

namespace WorkDelimiter.Model
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TasksOneTime
    {

        private TaskOneTime[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TaskOneTime", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TaskOneTime[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }
}
namespace WorkDelimiter.Model
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TaskOneTime : WorkDelimiter.Model.ITask //: INotifyPropertyChanged
    {
        private int idField;

        private string nameField;

        private System.DateTime creationDateField;

        private System.DateTime signalDateField;

        private byte isRegularField;

        private byte isActualField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime creationDate
        {
            get
            {
                return this.creationDateField;
            }
            set
            {
                this.creationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime signalDate
        {
            get
            {
                return this.signalDateField;
            }
            set
            {
                this.signalDateField = value;
                RaisePropertyChanged("signalDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte isRegular
        {
            get
            {
                return this.isRegularField;
            }
            set
            {
                this.isRegularField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte isActual
        {
            get
            {
                return this.isActualField;
            }
            set
            {
                this.isActualField = value;
                RaisePropertyChanged("isActual");
            }
        }
    }
    public partial class TaskOneTime : WorkDelimiter.Model.ITask, INotifyPropertyChanged
    {
        public byte _isExpired
        {
            get { if (signalDate > System.DateTime.Now) return 0; else return 1; }
            set { }
        }
        public byte IsExpired 
        {
            get { return _isExpired; }
            set { _isExpired = value; RaisePropertyChanged("IsExpired"); }
        }
        public TaskOneTime()
        {
            creationDate = System.DateTime.Now;
            signalDate = System.DateTime.Now;
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}