--2 lệnh đầu để chạy chỉ khi cần drop DB
ALTER DATABASE QL_CuaHangBanQuanAo SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE QL_CuaHangBanQuanAo
--
CREATE DATABASE QL_CuaHangBanQuanAo
USE QL_CuaHangBanQuanAo
--DROP TABLE
DROP TABLE ChiTietPhieuNhap
DROP TABLE PhieuNhap
DROP TABLE DanhGia
DROP TABLE ThanhToan
DROP TABLE ChiTietHoaDon
DROP TABLE HoaDon
DROP TABLE TrangThaiDon
DROP TABLE NhanVien
DROP TABLE KhachHang
DROP TABLE BienTheSanPham
DROP TABLE SanPham
DROP TABLE NhaCungCap
DROP TABLE KichThuoc
DROP TABLE LoaiSanPham
DROP TABLE TaiKhoan
DROP TABLE LoaiTaiKhoan
--CREATE TABLE
CREATE TABLE LoaiTaiKhoan
(
	MaLoaiTK INT IDENTITY(1,1),
	TenLoaiTK NVARCHAR(30),
	GhiChu NVARCHAR(150),
	--
	CONSTRAINT PK_LoaiTaiKhoan PRIMARY KEY(MaLoaiTK)
)
CREATE TABLE TaiKhoan
(
	MaTK INT IDENTITY(1,1),
	TenDangNhap NVARCHAR(50),
	MatKhau NVARCHAR(150),
	Email NVARCHAR(100),
	SDT VARCHAR(15),
	MaLoaiTK INT,--FK->LoaiTaiKhoan.MaLoaiTK
	--
	CONSTRAINT PK_TaiKhoan PRIMARY KEY(MaTK),
	CONSTRAINT FK_TaiKhoan_LoaiTaiKhoan FOREIGN KEY(MaLoaiTK) REFERENCES LoaiTaiKhoan(MaLoaiTK),
)
CREATE TABLE LoaiSanPham
(
	MaLoaiSP INT IDENTITY(1,1),
	TenLoaiSP NVARCHAR(50),
	CONSTRAINT PK_LoaiSanPham PRIMARY KEY(MaLoaiSP),
)
CREATE TABLE KichThuoc
(
	MaKT INT IDENTITY(1,1),
	TenKT NVARCHAR(50),
	CONSTRAINT PK_KichThuoc PRIMARY KEY(MaKT),
)
CREATE TABLE NhaCungCap
(
	MaNCC INT IDENTITY(1,1),
	TenNCC NVARCHAR(50),
	DiaChi NVARCHAR(150),
	SDT VARCHAR(15),
	CONSTRAINT PK_NhaCungCap PRIMARY KEY(MaNCC),
)
CREATE TABLE SanPham
(
	MaSP INT IDENTITY(1,1),
	TenSP NVARCHAR(150),
	MaLoaiSP INT,--FK->LoaiSanPham.MaLoaiSP
	MaNCC INT,--FK->NhaCungCap.MaNCC
	GiaBan FLOAT,
	DuongDanAnh NVARCHAR(250),
	MoTa NVARCHAR(500),
	CONSTRAINT PK_SanPham PRIMARY KEY(MaSP),
	CONSTRAINT FK_SanPham_LoaiSanPham FOREIGN KEY(MaLoaiSP) REFERENCES LoaiSanPham(MaLoaiSP),
	CONSTRAINT FK_SanPham_NhaCungCap FOREIGN KEY(MaNCC) REFERENCES NhaCungCap(MaNCC),
)
CREATE TABLE BienTheSanPham
(
	MaBTSP INT IDENTITY(1,1),
	MaSP INT,--FK->SanPham.MaSP
	MaKT INT,--FK->KichThuoc.MaKT
	SoLuong INT,
	CONSTRAINT PK_BienTheSanPham PRIMARY KEY(MaBTSP),
	CONSTRAINT FK_BienTheSanPham_SanPham FOREIGN KEY(MaSP) REFERENCES SanPham(MaSP),
	CONSTRAINT FK_BienTheSanPham_KichThuoc FOREIGN KEY(MaKT) REFERENCES KichThuoc(MaKT),
)
CREATE TABLE KhachHang
(
	MaKH INT IDENTITY(1,1),
	Ho NVARCHAR(50),
	Ten NVARCHAR(30),
	NgaySinh DATE,
	DiaChi NVARCHAR(150),
	MaTK INT,--FK->TaiKhoan.MaTK
	CONSTRAINT PK_KhachHang PRIMARY KEY(MaKH),
	CONSTRAINT FK_KhachHang_TaiKhoan FOREIGN KEY(MaTK) REFERENCES TaiKhoan(MaTK),
)
CREATE TABLE NhanVien
(
	MaNV INT IDENTITY(1,1),
	Ho NVARCHAR(50),
	Ten NVARCHAR(30),
	NgaySinh DATE,
	MaTK INT,--FK->TaiKhoan.MaTK
	CONSTRAINT PK_NhanVien PRIMARY KEY(MaNV),
	CONSTRAINT FK_NhanVien_TaiKhoan FOREIGN KEY(MaTK) REFERENCES TaiKhoan(MaTK)
)
CREATE TABLE TrangThaiDon--Chờ duyệt, đã huỷ, đã đặt, đã nhận hàng
(
	MaTTD INT IDENTITY(1,1),
	TenTTD NVARCHAR(50),
	CONSTRAINT PK_TrangThaiDon PRIMARY KEY(MaTTD),
)
CREATE TABLE HoaDon
(
	MaHD INT IDENTITY(1,1),
	NgayDat DATE,
	MaKH INT,--FK->KhachHang.MaKH
	DiaChiGiaoHang NVARCHAR(100),
	MaTTD INT, --FK->TrangThaiDon.MaTTD
	TongTien FLOAT DEFAULT 0,
	MaNV INT,--FK->NhanVien.MaNV
	CONSTRAINT PK_HoaDon PRIMARY KEY(MaHD),
	CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY(MaKH) REFERENCES KhachHang(MaKH),
	CONSTRAINT FK_HoaDon_TrangThaiDon FOREIGN KEY(MaTTD) REFERENCES TrangThaiDon(MaTTD),
	CONSTRAINT FK_HoaDon_NhanVien FOREIGN KEY(MaNV) REFERENCES NhanVien(MaNV),
)
CREATE TABLE ChiTietHoaDon
(
	MaHD INT,--FK->HoaDon.MaDH
	MaSP INT,--FK->SanPham.MaSP
	SoLuong INT,
	GiaTien FLOAT DEFAULT 0,
	CONSTRAINT PK_ChiTietHoaDon PRIMARY KEY(MaHD,MaSP),
	CONSTRAINT FK_ChiTietHoaDon_HoaDon FOREIGN KEY(MaHD) REFERENCES HoaDon(MaHD),
	CONSTRAINT FK_ChiTietHoaDon_SanPham FOREIGN KEY(MaSP) REFERENCES SanPham(MaSP),
)
CREATE TABLE ThanhToan
(
	MaTT INT IDENTITY(1,1),
	MaHD INT,--FK->HoaDon.MaHD
	NgayThanhToan DATE,
	SoTienThanhToan FLOAT,
	CONSTRAINT PK_ThanhToan PRIMARY KEY(MaTT),
	CONSTRAINT FK_ThanhToan_HoaDon FOREIGN KEY(MaHD) REFERENCES HoaDon(MaHD),
)
CREATE TABLE DanhGia
(
	MaDG INT IDENTITY(1,1),
	MaSP INT,--FK->SanPham.MaSP
	MaKH INT,--FK->KhachHang.MaKH
	Diem INT,--1-5 Sao
	BinhLuan NVARCHAR(250),
	NgayDG DATE,
	CONSTRAINT PK_DanhGia PRIMARY KEY(MaDG),
	CONSTRAINT FK_DanhGia_SanPham FOREIGN KEY(MaSP) REFERENCES SanPham(MaSP),
	CONSTRAINT FK_DanhGia_KhachHang FOREIGN KEY(MaKH) REFERENCES KhachHang(MaKH),
)
CREATE TABLE PhieuNhap
(
	MaPN INT IDENTITY(1,1),--PK
	NgayNhap DATE NOT NULL DEFAULT GETDATE(),
	TongTien INT DEFAULT 0,
	MaNCC INT,--FK->NhaCungCap.MaNCC
	MaNV INT,--FK->NhanVien.MaNV
	--
	CONSTRAINT PK_PhieuNhap PRIMARY KEY(MaPN),
	CONSTRAINT FK_PhieuNhap_NhaCungCap FOREIGN KEY(MaNCC) REFERENCES NhaCungCap(MaNCC),
	CONSTRAINT FK_PhieuNhap_NhanVien FOREIGN KEY(MaNV) REFERENCES NhanVien(MaNV),
)
CREATE TABLE ChiTietPhieuNhap
(
	MaPN INT,--PK,FK->NhapHang.MaPN
	MaSP INT,--PK,FK->SanPham.MaSP
	SoLuong INT,
	DonGiaNhap INT
	--
	CONSTRAINT PK_ChiTietPhieuNhap PRIMARY KEY(MaPN,MaSP),
	CONSTRAINT FK_ChiTietPhieuNhap_PhieuNhap FOREIGN KEY(MaPN) REFERENCES PhieuNhap(MaPN),
	CONSTRAINT FK_ChiTietPhieuNhap_SanPham FOREIGN KEY(MaSP) REFERENCES SanPham(MaSP),
)
--
INSERT INTO LoaiTaiKhoan VALUES
(N'Người dùng','Người dùng với những quyền cơ bản này kia'),
(N'Quản lý','Admin'),
(N'Siêu quản lý','SuperAdmin')
--
INSERT INTO TaiKhoan VALUES
(N'superadmin','123','superadmin@gmail.com','',3),
(N'admin','123','admin@gmail.com','',2),
(N'hdang','123','hdang@yahoo.com','',1),
(N'hminh','321','hminh@gmail.com','',1),
(N'akhoa','132','akhoa@gmail.com','',1),
(N'mluan','213','mluan@gmail.com','',1),
(N'kobebryant','248','imurfankobeee@yahoo.com','',1)
--
INSERT INTO LoaiSanPham VALUES
(N'Áo thun'),
(N'Áo polo'),
(N'Áo sơ mi'),
(N'Áo khoác'),
(N'Áo len'),
(N'Áo hoodie'),
(N'Quần jeans'),
(N'Quần âu'),
(N'Quần short'),
(N'Giày')
--
INSERT INTO KichThuoc VALUES
(N'S'),
(N'M'),
(N'L'),
(N'XL'),
(N'XXL'),
(N'Freesize')
--
INSERT INTO NhaCungCap VALUES
(N'Outerity','Tp. HCM', ''),
(N'BadHabits','Tp. HCM', ''),
(N'BadRabbit','Tp. HCM', ''),
(N'Yame','Tp. HCM', ''),
(N'Dirtycoins','Tp. HCM', ''),
(N'Uniqlo','Tp. HCM', '')
--
INSERT INTO SanPham(TenSP,MaLoaiSP,MaNCC,GiaBan,DuongDanAnh,SoLuong,MoTa) VALUES
(N'Áo thun Outerity Blanki / Gray Pinstrip',1,1,230000,'SP001.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Áo thun Outerity Blanki / Heather Black',1,1,230000,0,'SP002.jpg',N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Áo thun Outerity Blanki / Heather Gray',1,1,230000,0,'SP003.jpg',N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Áo thun Outerity Blanki / Navy Peony',1,1,230000,0,'SP004.jpg',N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'MULTIFONT TEE - Dark Green Color',1,1,230000,0,'SP005.jpg',N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Outerity BabyMonster Tee - Meow Collection / Red',1,1,230000,0,'SP006.jpg',N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'CROSS CUT POLO / Black-Grey Color',2,1,275000,'SP007.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'CROSS CUT POLO / White-Grey Color',2,1,275000,'SP008.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'POLO YAME / Cream Color',2,4,350000,'SP009.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'BOMBER YAME / Black Color',4,4,550000,'SP0010.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'BOMBER DIRTY COINS / GREEN Color',4,5,650000,'SP0011.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'JEAN JACKET / Grey Color',4,2,470000,'SP0012.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Quần Short / Black Color',9,5,200000,'SP0013.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Quần JEAN / Grey Color',7,5,650000,'SP0014.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Quần Bóng Rổ / Black Color',9,5,200000,'SP0015.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'GAM TIME / Cream Color',1,5,475000,'SP0016.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.'),
(N'Quần Âu BadHabits/ Black Color',8,2,450000,'SP0017.jpg',0,N'🔹 Bảng size Outerity
S: Dài 69 Rộng 52.5 | 1m50 - 1m65, 45 - 55Kg
M: Dài 73 Rộng 55 | 1m60 - 1m75, 50 - 65Kg
L: Dài 76.5 Rộng 57.5 | 1m7 - 1m8, 65Kg - 80Kg
👉 Nếu chưa biết lựa size bạn có thể inbox để được chúng mình tư vấn.')
--
INSERT INTO BienTheSanPham VALUES
(1,1,0),
(1,2,0),
(1,3,0),
(2,1,0),
(2,2,0),
(2,3,0),
(3,1,0),
(3,2,0),
(3,3,0),
(4,1,0),
(4,2,0),
(4,3,0),
(5,1,0),
(5,2,0),
(5,3,0),
(6,1,0),
(6,2,0),
(6,3,0),
(7,1,0),
(7,2,0),
(7,3,0),
(8,1,0),
(8,2,0),
(8,3,0)
--
INSERT INTO KhachHang VALUES
(N'Nguyễn Hải',N'Đăng','2004-08-18',N'',3),
(N'Nguyễn Hải',N'Minh','2004-01-01',N'',4),
(N'Đoàn Nguyễn Anh',N'Khoa','2004-01-02',N'',5),
(N'Nguyễn Minh',N'Luân','2004-01-03',N'',6),
(N'Kobe',N'Bryant','2000-01-04',N'Thiên đàng',7)
--
INSERT INTO NhanVien VALUES
(N'Dương Hoàng Văn','Bá','2004-08-18',1),
(N'Hoàng Nguyễn Quang','Bách','2004-05-20',2)
--
INSERT INTO TrangThaiDon VALUES
(N'Chờ duyệt'),
(N'Đã huỷ'),
(N'Đã đặt'),
(N'Đã giao')
--
SELECT * FROM LoaiTaiKhoan;
SELECT * FROM TaiKhoan;
SELECT * FROM LoaiSanPham;
SELECT * FROM KichThuoc;
SELECT * FROM NhaCungCap;
SELECT * FROM SanPham;
SELECT * FROM BienTheSanPham;
SELECT * FROM KhachHang;
SELECT * FROM NhanVien;
SELECT * FROM TrangThaiDon;
SELECT * FROM HoaDon;
SELECT * FROM ChiTietHoaDon;
SELECT * FROM ThanhToan;
SELECT * FROM DanhGia;
SELECT * FROM PhieuNhap;
SELECT * FROM ChiTietPhieuNhap;
--


