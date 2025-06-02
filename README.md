# Hệ Thống Quản Lý Công Thức Nấu Ăn

## 5. Các Tính Năng Chính

### 5.1. Quản Lý Công Thức

#### CRUD Operations cho Công Thức
- **Tạo công thức mới**
  - Thêm thông tin cơ bản: tên, công thức
  - Gán các thẻ (tags) liên quan

- **Xem công thức**
  - Hiển thị đầy đủ thông tin công thức
  - Hiển thị danh sách đánh giá

- **Cập nhật công thức**
  - Chỉnh sửa thông tin cơ bản
  - Thêm/xóa/sửa nguyên liệu
  - Thêm/xóa/sửa các bước nấu
  - Cập nhật hình ảnh
  - Quản lý thẻ

- **Xóa công thức**
  - Xóa vĩnh viễn
  - Lưu vào thùng rác (soft delete)
  - Khôi phục từ thùng rác

#### Tìm Kiếm và Lọc Công Thức
- **Tìm kiếm theo từ khóa**
  - Tìm theo tên công thức
  - Tìm theo nguyên liệu
  - Tìm theo mô tả
  - Tìm theo thẻ

- **Bộ lọc nâng cao**
  - Lọc theo thời gian nấu
  - Lọc theo độ khó
  - Lọc theo loại món ăn
  - Lọc theo chế độ ăn (chay, mặn, etc.)
  - Lọc theo mùa
  - Lọc theo xuất xứ

#### Phân Trang Kết Quả
- Hiển thị số lượng kết quả mỗi trang (10, 20, 50 items)
- Điều hướng trang (first, previous, next, last)
- Hiển thị tổng số trang và tổng số kết quả
- Lưu trạng thái phân trang

### 5.2. Quản Lý Đánh Giá

#### Thêm/Xóa/Sửa Đánh Giá
- **Thêm đánh giá mới**
  - Chấm điểm (1-5 sao)
  - Viết nhận xét
  - Thêm hình ảnh minh họa
  - Đánh dấu hữu ích/không hữu ích

- **Chỉnh sửa đánh giá**
  - Sửa điểm đánh giá
  - Sửa nội dung nhận xét
  - Cập nhật hình ảnh

- **Xóa đánh giá**
  - Xóa vĩnh viễn
  - Ẩn đánh giá (cho người dùng khác)

#### Tính Toán Điểm Trung Bình
- Tính điểm trung bình cho mỗi công thức
- Phân loại đánh giá (tốt, khá, trung bình, kém)
- Thống kê số lượng đánh giá theo mức độ
- Cập nhật điểm trung bình realtime
- Sắp xếp công thức theo điểm đánh giá

### 5.3. Quản Lý Thẻ

#### CRUD Operations cho Thẻ
- **Tạo thẻ mới**
  - Đặt tên thẻ
  - Thêm mô tả
  - Chọn màu sắc đại diện
  - Thêm icon

- **Xem danh sách thẻ**
  - Hiển thị tất cả thẻ
  - Hiển thị số lượng công thức cho mỗi thẻ
  - Sắp xếp theo tên/số lượng

- **Cập nhật thẻ**
  - Sửa tên thẻ
  - Sửa mô tả
  - Thay đổi màu sắc
  - Cập nhật icon

- **Xóa thẻ**
  - Xóa vĩnh viễn
  - Xóa và chuyển công thức sang thẻ khác

#### Gán Thẻ cho Công Thức
- Gán nhiều thẻ cho một công thức
- Gỡ thẻ khỏi công thức
- Tự động gợi ý thẻ dựa trên nội dung
- Kiểm tra trùng lặp thẻ

#### Tìm Kiếm Công Thức Theo Thẻ
- Tìm công thức theo một thẻ
- Tìm công thức theo nhiều thẻ (AND/OR)
- Hiển thị các thẻ liên quan
- Gợi ý thẻ phổ biến 
