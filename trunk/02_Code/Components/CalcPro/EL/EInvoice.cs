using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class EInvoice
    {
        private int _ProjectID = -1;
        private int _InvoiceID = -1;
        private string _InvoiceNumber;
        private string _InvoiceDescription;
        private string _InvoiceType;
        private DataTable _dtBlattNumbers;
        private DataTable _dtInvoice;
        private bool _IsFinalInvoice;
        private DataTable _dtInvoices;

        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }
        public string InvoiceNumber
        {
            get { return _InvoiceNumber; }
            set { _InvoiceNumber = value; }
        }
        public string InvoiceDescription
        {
            get { return _InvoiceDescription; }
            set { _InvoiceDescription = value; }
        }
        public string InvoiceType
        {
            get { return _InvoiceType; }
            set { _InvoiceType = value; }
        }
        public DataTable dtBlattNumbers
        {
            get { return _dtBlattNumbers; }
            set { _dtBlattNumbers = value; }
        }
        public DataTable dtInvoice
        {
            get { return _dtInvoice; }
            set { _dtInvoice = value; }
        }
        public DataTable dtInvoices
        {
            get { return _dtInvoices; }
            set { _dtInvoices = value; }
        }
        public bool IsFinalInvoice
        {
            get { return _IsFinalInvoice; }
            set { _IsFinalInvoice = value; }
        }

    }
}
