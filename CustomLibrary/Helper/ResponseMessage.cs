using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomLibrary.Helper
{
    public partial class ResponseMessage
    {
        public string? Header { get; set; }
        public string? Detail { get; set; }
        public string Note { get; set; } = "";
        public dynamic? Data { get; set; }
    }

    public static class ResponseMessageExtensions
    {
        public static class Database
        {
            public const string DataNotFound = "Data Tidak Dapat Ditemukan";
            public const string DataNotValid = "Data Tidak Valid, Harap Cek Kembali";
            public const string PasswordNotValid = "Password tidak valid, harap cek kembali";
            public const string DataAlreadyExist = "Data Sudah Ada";
            public const string DeleteSuccess = "Data Berhasil Dihapus";
            public const string UpdateSuccess = "Data Berhasil Diubah";
            public const string UpdateFailed = "Data Gagal Diubah";
            public const string WriteSuccess = "Data Berhasil Disimpan";
            public const string WriteFailed = "Data Gagal Disimpan";
        }
        public static class Category
        {
            public const string ClassDifferent = "Kelas tidak sesuai";
            public const string CategoryNotFound = "Produk kategori tidak dapat ditemukan, harap periksa kembali";
            public const string CategoryUsedInProduct = "Kategori digunakan di produk";
            public const string CategoryAlreadyExist = "Produk kategori sudah terdaftar";
        }
        public static class Product
        {
            public const string ProductNotFound = "Produk tidak dapat ditemukan, harap periksa kembali";
            public const string ProductUsedInVariant = "Produk digunakan di produk varian";
            public const string ProductAlreadyExist = "Produk sudah terdaftar";
        }
        public static class Variant
        {
            public const string ProductVariantNotFound = "Variasi produk tidak dapat ditemukan, harap periksa kembali";
            public const string ProductVariantUsedInTransactionDetail = "Variasi produk digunakan di transaksi detail";
            public const string ProductVariantAlreadyExist = "Variasi produk sudah terdaftar";
        }
        public static class File
        {
            public const string DefaultError = "File Error";
            public const string FileNotFound = "File Tidak Ditemukan";
            public const string UploadSuccess = "Berhasil Upload File";
            public const string UploadFailed = "Gagal Upload File";
            public const string ExtensionNotAllowed = "File Tidak Diperbolehkan Untuk Di Upload";
            public const string SizeOverLimit = "Ukuran File Melebihi Maksimal Yang Di Perbolehkan";
            public const string InvalidSignature = "Signature File Error";
            public const string SizeNotValid = "File Tidak Bisa Diproses";
            public const string CreationError = "Tidak Dapat Menyimpan File";
            public const string ThumbCreationError = "Tidak Dapat Membuat Thumbnail";
            public const string DeleteSuccess = "Berhasil Menghapus File";
            public const string DeleteFailed = "Gagal Menghapus File";
        }
        public static class Transaction
        {
            public const string PriceDifferent = "Harga tidak sesuai";
            public const string TransactionNotFound = "Transaksi tidak dapat ditemukan";
            public const string TransactionCantClosed = "Masih terdapat rit yang berjalan, tidak dapat menutup transaksi";
            public const string PriceHasBeenUsedInTrayek = "Harga sudah digunakan di trayek";
        }
        public static class Access
        {
            public const string Unauthorized = "Unauthorized";
            public const string FeatureDisabled = "Fitur Di Non Aktifkan";
            public const string ResourceConflict = "Resource ini bukan milik anda";
            public const string Prohibited = "Anda Tidak Mempunyai Akses";
        }
        public static class Controllers
        {
            public const string ControllerNotFound = "Controller tidak ditemukan";
            public const string ConstraintNotFound = "Jenis kendala tidak dapat ditemukan, harap periksa kembali";
            public const string ConstraintAlreadyExist = "Jenis kendala sudah tersedia, harap periksa kembali";
        }

        public const string SuccessHeader = "Sukses!";
        public const string FailHeader = "Gagal!";
        public const string DefaultDetailMessage = "Hubungi Administrator";

        private static class ContentType
        {
            public const string ApplicationJson = "application/json";
        }

        public static ObjectResult InternalServerError(this ControllerBase controller, string message = null)
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
            {
                Header = FailHeader,
                Detail = message ?? DefaultDetailMessage
            });
        }

        public static OkObjectResult OkResponse(this ControllerBase controller, string message, string? note = null, dynamic? data = null)
        {
            return controller.Ok(new ResponseMessage
            {
                Header = SuccessHeader,
                Detail = message,
                Note = note ?? "",
                Data = data ?? null
            });
        }

        public static ObjectResult AcceptedResult(this ControllerBase controller, string message, string? note = null, dynamic? data = null)
        {
            return controller.Accepted(new ResponseMessage
            {
                Header = SuccessHeader,
                Detail = message,
                Note = note ?? "",
                Data = data ?? null
            });
        }

        public static async Task BadRequestResponse(this HttpResponse response, string message, string? note = null, dynamic? data = null)
        {
            response.ContentType = ContentType.ApplicationJson;
            response.StatusCode = StatusCodes.Status400BadRequest;
            var responseMessage = new ResponseMessage
            {
                Header = FailHeader,
                Detail = message,
                Note = note ?? "",
                Data = data
            };

            await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
        }

        public static async Task InternalErrorResponse(this HttpResponse response, string message)
        {
            response.ContentType = ContentType.ApplicationJson;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            var responseMessage = new ResponseMessage
            {
                Header = FailHeader,
                Detail = DefaultDetailMessage,
                Note = message
            };

            await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
        }

        public static async Task UnathourizedResponse(this HttpResponse response, string message)
        {
            response.ContentType = ContentType.ApplicationJson;
            response.StatusCode = StatusCodes.Status401Unauthorized;
            var responseMessage = new ResponseMessage
            {
                Header = FailHeader,
                Detail = Access.Prohibited,
                Note = message
            };

            await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
        }

        public static async Task ServiceUnavailableResponse(this HttpResponse response, string message)
        {
            response.ContentType = ContentType.ApplicationJson;
            response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            var responseMessage = new ResponseMessage
            {
                Header = FailHeader,
                Detail = message
            };

            await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
        }

        public static async Task ForbiddedResponse(this HttpResponse response)
        {
            response.ContentType = ContentType.ApplicationJson;
            response.StatusCode = StatusCodes.Status403Forbidden;
            var responseMessage = new ResponseMessage
            {
                Header = FailHeader,
                Detail = "Anda Tidak Mempunyai Akses Terhadap Resource Ini"
            };

            await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
        }
    }
}
