using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OutOfOrderGenerator.ViewModels
{
    public class OneViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private string nowTime { get; set; }
        public string NowTime
        {
            get { return nowTime; }
            set
            {
                if(nowTime == value) { return; }
                nowTime = value;
                Notify("nowTime");
            }
        }

        //计量单号
        private string num { get; set; }
        public string Num
        {
            get { return num; }
            set
            {
                if (num == value) { return; }
                num = value;
                Notify("num");
            }
        }

        //车号
        private string carNum { get; set; }
        public string CarNum
        {
            get { return carNum; }
            set
            {
                if (carNum == value) { return; }
                carNum = value;
                Notify("carNum");
            }
        }

        //凭证号
        private string certificateNum { get; set; }
        public string CertificateNum
        {
            get { return certificateNum; }
            set
            {
                if (certificateNum == value) { return; }
                certificateNum = value;
                Notify("certificateNum");
            }
        }

        //行项目号
        private string lineItemsNum { get; set; }
        public string LineItemsNum
        {
            get { return lineItemsNum; }
            set
            {
                if (lineItemsNum == value) { return; }
                lineItemsNum = value;
                Notify("lineItemsNum");
            }
        }

        //物料编号
        private string materialCode { get; set; }
        public string MaterialCode
        {
            get { return materialCode; }
            set
            {
                if (materialCode == value) { return; }
                materialCode = value;
                Notify("materialCode");
            }
        }

        //物料名称
        private string materialName { get; set; }
        public string MaterialName
        {
            get { return materialName; }
            set
            {
                if (materialName == value) { return; }
                materialName = value;
                Notify("materialName");
            }
        }

        //供方
        private string providerName { get; set; }
        public string ProviderName
        {
            get { return providerName; }
            set
            {
                if (providerName == value) { return; }
                providerName = value;
                Notify("materialName");
            }
        }

        //收方
        private string collectName { get; set; }
        public string CollectName
        {
            get { return collectName; }
            set
            {
                if (collectName == value) { return; }
                collectName = value;
                Notify("materialName");
            }
        }

        //毛重
        private string grossWeight { get; set; }
        public string GrossWeight
        {
            get { return grossWeight; }
            set
            {
                if (grossWeight == value) { return; }
                grossWeight = value;
                Notify("grossWeight");
                ChangeGrainWeight();
                ChangNetWeight();
            }
        }

        //皮重
        private string tare { get; set; }
        public string Tare
        {
            get { return tare; }
            set
            {
                if (tare == value) { return; }
                tare = value;
                Notify("tare");
                ChangeGrainWeight();
                ChangNetWeight();
            }
        }

        //粮重
        private string grainWeight { get; set; }
        public string GrainWeight
        {
            get { return grainWeight; }
            set
            {
                if (grainWeight == value) { return; }
                grainWeight = value;
                Notify("grainWeight");
            }
        }

        //扣重
        private string buckleWeight { get; set; }
        public string BuckleWeight
        {
            get { return buckleWeight; }
            set
            {
                if (buckleWeight == value) { return; }
                buckleWeight = value;
                Notify("buckleWeight");
                ChangNetWeight();
            }
        }

        //净重
        private string netWeight { get; set; }
        public string NetWeight
        {
            get { return netWeight; }
            set
            {
                if (netWeight == value) { return; }
                netWeight = value;
                Notify("netWeight");
            }
        }

        //毛重和皮重改变时修改粮重
        public void ChangeGrainWeight()
        {
            double dgrossWeight, dTare;
            bool success1 = double.TryParse(tare, out dTare);
            bool success2 = double.TryParse(grossWeight, out dgrossWeight);
            if (success1 && success2)
            {
                GrainWeight = (dgrossWeight - dTare).ToString();
            }
        }

        //毛重、皮重、扣重改变时修改净重
        public void ChangNetWeight()
        {
            double dgrossWeight, dTare, dBuckle;
            bool success1 = double.TryParse(tare, out dTare);
            bool success2 = double.TryParse(grossWeight, out dgrossWeight);
            bool success3 = double.TryParse(buckleWeight, out dBuckle);
            if(success1 && success2 && success3)
            {
                NetWeight = ((dgrossWeight - dTare) * (1 - dBuckle / 100.0)).ToString();
            }
        }
    }
}
