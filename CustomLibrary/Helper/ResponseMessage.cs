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
            public const string WriteSuccess = "Data Berhasil Disimpan";
            public const string WriteFailed = "Data Gagal Disimpan";
        }
        public static class Bank
        {
            public const string BankDifferent = "Bank Tidak Sesuai";
            public const string BankNotFound = "Bank Tidak Dapat Ditemukan, Harap Periksa Kembali";
            public const string BankIsUsed = "Bank digunakan untuk credential bank";
            public const string BankAlreadyExist = "Data Bank Sudah Ada";
            public const string DeleteSuccess = "Data Deleted";
            public const string UpdateSuccess = "Data Updated";
            public const string WriteSuccess = "Data Saved";
        }
        public static class CredentialBank
        {
            public const string CredentialDifferent = "Credential Bank Tidak Sesuai";
            public const string CredentialNotFound = "Credential Bank Tidak Dapat Ditemukan, Harap Periksa Kembali";
            public const string CredentialAlreadyExist = "Data Credential Bank Sudah Ada";
            public const string CredentialInUsed = "Credential bank sudah digunakan pada kondektur";
            public const string DeleteSuccess = "Data Deleted";
            public const string UpdateSuccess = "Data Updated";
            public const string WriteSuccess = "Data Saved";
        }
        public static class Vehicle
        {
            public const string VehicleDifferent = "Kendaraan Tidak Sesuai";
            public const string VehicleNotFound = "Kendaraan Tidak Dapat Ditemukan, Harap Periksa Kembali";
            public const string VehicleAlreadyExist = "Data Kendaraan Sudah Ada";
            public const string VehicleAlreadyUsedInTrayek = "Kendaraan sudah digunakan di trayek";
            public const string VehicleAlreadyUsedInSchedule = "Kendaraan sudah digunakan di jadwal bus";
            public const string VehicleAlreadyUsedInTransaction = "Kendaraan sudah digunakan di jadwal yang sedang berjalan";
            public const string DeleteSuccess = "Data Deleted";
            public const string UpdateSuccess = "Data Updated";
            public const string WriteSuccess = "Data Saved";
        }
        public static class Class
        {
            public const string ClassDifferent = "Kelas tidak sesuai";
            public const string ClassNotFound = "Kelas tidak dapat ditemukan, harap periksa kembali";
            public const string ClassUsedInTrayek = "Kelas digunakan di trayek";
            public const string ClassAlreadyExist = "Data kelas sudah ada";
            public const string DeleteSuccess = "Data Deleted";
            public const string UpdateSuccess = "Data Updated";
            public const string WriteSuccess = "Data Saved";
        }
        public static class Rits
        {
            public const string RitNotFound = "Rit tidak ditemukan";
        }
        public static class Station
        {
            public const string StationDifferent = "Terminal/Halte tidak sesuai";
            public const string StationNotFound = "Terminal/Halte tidak dapat ditemukan, harap periksa kembali";
            public const string StationUsedInPrice = "Terminal/Halte sudah dipakai di rute";
            public const string StationAlreadyExist = "Data terminal/halte sudah ada";
            public const string DeleteSuccess = "Data Deleted";
            public const string UpdateSuccess = "Data Updated";
            public const string WriteSuccess = "Data Saved";
            public const string RouteNotUsedInTrayek = "Rute tidak digunakan di trayek";
        }
        public static class Route
        {
            public const string RouteDifferent = "Rute tidak sesuai";
            public const string RouteNotFound = "Rute tidak dapat ditemukan, harap periksa kembali";
            public const string RouteIsUsedInRoute = "Rute digunakan di rute";
            public const string RouteIsUsed = "Rute digunakan diharga";
            public const string RouteAlreadyExist = "Data Rute sudah ada";
            public const string RouteFoundInPrice = "Rute sudah digunakan di harga, harap gunakan perubahan harga";
            public const string RouteAvailable = "Rute dapat digunakan";
        }
        public static class Trayek
        {
            public const string RouteDifferent = "Rute tidak sesuai";
            public const string TrayekIsUsedInSchedule = "Trayek digunakan di penjadwalan";
            public const string TrayekIsUsedInPrice = "Trayek digunakan di harga";
            public const string DeleteSuccess = "Data Deleted";
            public const string UpdateSuccess = "Data Updated";
            public const string WriteSuccess = "Data Saved";
        }
        public static class Passenger
        {
            public const string PassengerDifferent = "Jenis penumpang tidak sesuai";
            public const string PassengerNotFound = "Jenis penumpang tidak dapat ditemukan, harap periksa kembali";
            public const string PassengerAlreadyExist = "Data jenis penumpang sudah ada";
            public const string DeleteSuccess = "Data Deleted";
            public const string UpdateSuccess = "Data Updated";
            public const string WriteSuccess = "Data Saved";
            public const string PassengerInPrice = "Jenis penumpang digunakan di harga";
        }
        public static class Expense
        {
            public const string ExpenseTypeDifferent = "Tipe pengeluaran tidak sesuai";
            public const string ExpenseTypeNotFound = "Tipe pengeluaran tidak dapat ditemukan, harap periksa kembali";
            public const string ExpenseTypeAlreadyExist = "Data tipe pengeluaran sudah ada";
            public const string ExpenseDifferent = "Pengeluaran tetap tidak sesuai";
            public const string ExpenseNotFound = "Pengeluaran tetap tidak dapat ditemukan, harap periksa kembali";
            public const string ExpenseRevisiNotFound = "Revisi pengeluaran tidak dapat ditemukan, harap periksa kembali";
            public const string ExpenseAlreadyExist = "Data pengeluaran tetap sudah ada";
            public const string ExpenseUsedInTrayek = "Pengeluaran tetap digunakan di trayek";
        }
        public static class Price
        {
            public const string PriceDifferent = "Harga tidak sesuai";
            public const string PriceNotFound = "Harga tidak dapat ditemukan, harap periksa kembali";
            public const string PriceAlreadyExist = "Data harga sudah ada";
            public const string PriceHasBeenUsedInTrayek = "Harga sudah digunakan di trayek";
            public const string OriginAlreadyInUsed = "Sudah terdapat origin di detail harga";
            public const string WrongDates = "Tanggal mulai lebih lambat dari tanggal akhir";
        }
        public static class Schedule
        {
            public const string ConductorNotAvailable = "Kondektur sudah digunakan pada jadwal lain";
            public const string ConductorAvailable = "Kondektur dapat dipilih";
            public const string VehicleNotAvailable = "Kendaraan sudah digunakan pada jadwal lain";
            public const string VehicleAvailable = "Kendaraan dapat dipilih";
            public const string ScheduleNotFound = "Jadwal tidak ditemukan";
            public const string ScheduleHasBeenUsed = "Jadwal sedang berjalan tidak dapat dihapus";
        }
        public static class Ticket
        {
            public const string TicketNotFound = "Tiket tidak ditemukan";
            public const string TicketStatusNotOper = "Tiket tidak dapat dioper karena memiliki status refund";
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
            public const string TransactionNotFound = "Transaksi rit tidak dapat ditemukan";
            public const string TransactionCantClosed = "Masih terdapat rit yang berjalan, tidak dapat menutup transaksi";
            public const string PriceHasBeenUsedInTrayek = "Harga sudah digunakan di trayek";
        }

        public static class Verbal
        {
            public const string VerbalNotFound = "Laporan verbal tidak ditemukan";
        }

        public static class Plotting
        {
            public const string PlottingValid = "Valid";
        }

        public static class Access
        {
            public const string Unauthorized = "Unauthorized";
            public const string FeatureDisabled = "Fitur Di Non Aktifkan";
            public const string ResourceConflict = "Resource ini bukan milik anda";
            public const string Prohibited = "Anda Tidak Mempunyai Akses";
        }

        public static class Constraint
        {
            public const string ConstraintDifferent = "Jenis kendala tidak sesuai";
            public const string ConstraintNotFound = "Jenis kendala tidak dapat ditemukan, harap periksa kembali";
            public const string ConstraintAlreadyExist = "Jenis kendala sudah tersedia, harap periksa kembali";
        }

        public static class TrayekType
        {
            public const string TrayekTypeDifferent = "Tipe Trayek tidak sesuai";
            public const string TrayekTypeNotFound = "Tipe Trayek tidak dapat ditemukan, harap periksa kembali";
            public const string TrayekTypeAlreadyExist = "Tipe Trayek sudah tersedia, harap periksa kembali";
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
