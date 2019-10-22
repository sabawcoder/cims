﻿using CIMS.Models;
using CIMS.ViewModels.DatabaseConnection.CRUD;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using CIMS.Views;
using Application = System.Windows.Application;
using System.Windows.Media.Imaging;

namespace CIMS.ViewModels.HelperClasses
{
    public class UnitProgressHelperClass
    {
        private readonly UniversalHelper helper = new UniversalHelper();
        private readonly UnitViewModel thisVM = new UnitViewModel();
        private readonly UnitProgressModel thisModel = new UnitProgressModel();
        private UnitProgressModel newModel = new UnitProgressModel();

        public UnitProgressHelperClass(UnitProgressModel thisModel, UnitViewModel thisVM)
        {
            this.thisModel = thisModel;
            this.thisVM = thisVM;
            PassValuesToControls();
        }

        private void PassValuesToControls()
        {
            if (thisModel == null) return;
            thisVM.UnitName = thisModel.UnitName;
            thisVM.UnitAddress = thisModel.UnitAddress;
            thisVM.SelectedUnitType = thisModel.UnitTypeName;
            thisVM.SelectedUnitStatuse = thisModel.UnitStatusName;
            thisVM.Notes = thisModel.Notes;
            thisVM.UnitImage = thisModel.Image;
            thisVM.ConvertedImage = ByteArrayToImage(thisModel.Image);

        }

        public void SaveItem(UnitProgressModel updatedModel)
        {
            this.newModel = updatedModel;
            if(!RecordExists())
            {
                Create.UnitProgress(updatedModel);
                helper.MessageDialog("Unit Progress data has been added successfully!",
                    updatedModel.UnitName);
            }
        }
        public bool RecordExists()
        {
            if (thisModel == null) return false;
            bool result = thisModel.ID != 0;
            return result;
        }

        public string ImageFilePath()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"C:\\";
            dlg.Title = "Select Image File";
            dlg.Filter = "Image Files  (*.jpg ; *.jpeg ; *.png ; *.gif ; *.tiff ; *.nef) " +
                                        "| *.jpg; *.jpeg; *.png; *.gif; *.tiff; *.nef";
            dlg.ShowDialog();
            return dlg.FileName;
        }


        public byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }
        private byte[] ImageToByteArray(Image imageIn)
        {
            if (imageIn == null) return null;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private BitmapImage ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(byteArrayIn))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }



    }
}
