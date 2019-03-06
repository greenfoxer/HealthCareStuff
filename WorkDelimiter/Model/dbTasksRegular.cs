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
using System.Threading;
namespace WorkDelimiter.Model
{



    // 
    // Этот исходный код был создан с помощью xsd, версия=4.0.30319.33440.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TasksRegular
    {

        private TaskRegular[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TaskRegular", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TaskRegular[] Items
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
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TaskRegular : WorkDelimiter.Model.ITask
    {
        private int idField;

        private string nameField;

        private System.DateTime creationDateField;

        private int periodField;

        private byte isRegularField;

        private int delimiterField;

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
        public int period
        {
            get
            {
                return this.periodField;
            }
            set
            {
                this.periodField = value;
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
        public int delimiter
        {
            get
            {
                return this.delimiterField;
            }
            set
            {
                this.delimiterField = value;
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
    public partial class TaskRegular : WorkDelimiter.Model.ITask, INotifyPropertyChanged
    {
        System.DateTime _nextTime;
        public System.DateTime NextTime
        { get { return _nextTime; } set { _nextTime = value; RaisePropertyChanged("NextTime"); } }
        System.DateTime _startTime;
        public System.DateTime StartTime
        { get { return _startTime; } set { _startTime = value; RaisePropertyChanged("StartTime"); } }
        System.DateTime _delimeterTime;
        public System.DateTime DelimiterTime
        { get { return _delimeterTime; } set { _delimeterTime = value; RaisePropertyChanged("DelimiterTime"); } }
        public void UpdateTime(System.DateTime start)
        {
            StartTime = start;
            DelimiterTime = StartTime.AddMinutes(period);
            NextTime = DelimiterTime.AddMinutes(delimiter);
        }
        public TaskRegular()
        {
            creationDate = System.DateTime.Now;
        }
        Thread Ticker;
        public bool IsTick { get; set; }
        public void StartTicker()
        {
            if (Ticker != null)
                Ticker.Abort();
            IsTick = true;
            Ticker = new Thread(() =>
            {
                UpdateTime(System.DateTime.Now);
                while (true)
                {
                    if(System.DateTime.Now>NextTime)
                        UpdateTime(System.DateTime.Now);
                    Thread.Sleep(60000);
                }
            });
            Ticker.IsBackground = true;
            Ticker.Start();
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