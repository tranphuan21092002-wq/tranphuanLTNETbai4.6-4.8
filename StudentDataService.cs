using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgressBarApp
{
    public class StudentDataService
    {
        public async Task<List<Student>> LoadStudentsAsync(IProgress<int>? progress = null)
        {
            // Thực thi tác vụ nặng trong Background Thread
            return await Task.Run(() =>
            {
                List<Student> list = new List<Student>();
                int totalItems = 100;

                for (int i = 1; i <= totalItems; i++)
                {
                    // Giả lập đọc từng bản ghi từ CSDL/File (50ms / bản ghi)
                    Thread.Sleep(50);

                    list.Add(new Student
                    {
                        Id = $"SV{i:D3}",
                        FullName = $"Sinh viên {i}",
                        ClassName = i % 2 == 0 ? "CNTT01" : "CNTT02",
                        Gpa = Math.Round(2.0 + (i % 20) * 0.1, 2)
                    });

                    // Tính phần trăm và truyền về UI Thread qua IProgress
                    int percent = (int)((double)i / totalItems * 100);
                    progress?.Report(percent);
                }

                return list;
            });
        }
    }
}