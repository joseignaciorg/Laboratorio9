using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Laboratorio9.Utils
{
    public class Storage
    {
        private CloudStorageAccount cuentaAlmacenamiento;

        public Storage(String cuenta, String clave)
        {
            StorageCredentials cred=new StorageCredentials(cuenta,clave);
            cuentaAlmacenamiento= new CloudStorageAccount(cred,true);
        }

        private void ComprobarContainer(String contenedor)
        {
            CloudBlobClient bobClient = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer container = bobClient.GetContainerReference(contenedor);
            
            container.CreateIfNotExists();  
        }

        public List<CloudBlockBlob> ListaContenedor(String contenedor)
        {
            ComprobarContainer(contenedor);
            CloudBlobClient blobClient = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(contenedor);

            List<CloudBlockBlob>urls=new List<CloudBlockBlob>();

            foreach (IListBlobItem item in container.ListBlobs(null,false))
            {
                if (item is CloudBlockBlob)
                {
                    var blob = item as CloudBlockBlob;
                    urls.Add(blob);
                }
            }

            return urls;
        }

        public void SubirFoto(Stream foto, String nombre, String contenedor)
        {
            ComprobarContainer(contenedor);
            CloudBlobClient blobClient = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer container=blobClient.GetContainerReference(contenedor);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(nombre);
            blockBlob.UploadFromStream(foto);
            foto.Close();
        }

        public void BorrarFoto(String foto, String contenedor)
        {
           ComprobarContainer(contenedor);
            CloudBlobClient blobClient = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(contenedor);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(foto);
            blockBlob.Delete();

        }
    }
}