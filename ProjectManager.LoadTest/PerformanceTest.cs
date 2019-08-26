using NBench;
using Newtonsoft.Json;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.LoadTest
{
    public class PerformanceTest
    {
        private HttpClient task_Client;
        private HttpClient user_Client;
        private HttpClient project_Client;
        private HttpClient parentTask_Client;
        private HttpResponseMessage _response;
        private const string serviceBaseURL_Task = "http://172.18.7.224:10/api/task/";
        private const string serviceBaseURL_User = "http://172.18.7.224:10/api/user/";
        private const string serviceBaseURL_Project = "http://172.18.7.224:10/api/project/";
        private const string serviceBaseURL_ParentTask = "http://172.18.7.224:10/api/parenttask/";

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            task_Client = new HttpClient { BaseAddress = new Uri(serviceBaseURL_Task) };
            user_Client = new HttpClient { BaseAddress = new Uri(serviceBaseURL_User) };
            project_Client = new HttpClient { BaseAddress = new Uri(serviceBaseURL_Project) };
            parentTask_Client = new HttpClient { BaseAddress = new Uri(serviceBaseURL_ParentTask) };
        }

        [PerfBenchmark(Description = "Test to ensure that a minimal throughput test can be rapidly executed.",
        NumberOfIterations = 3, RunMode = RunMode.Throughput,
        RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, ByteConstants.ThirtyTwoKb)]
        [GcTotalAssertion(GcMetric.TotalCollections, GcGeneration.Gen2, MustBe.ExactlyEqualTo, 0.0d)]
        public void Benchmark_GetTaskDetails()
        {
            _response = task_Client.GetAsync(serviceBaseURL_Task).Result;
            var responseResult =
                JsonConvert.DeserializeObject<List<view_TaskSearch>>(_response.Content.ReadAsStringAsync().Result);
        }

        [PerfBenchmark(Description = "Test to ensure that a minimal throughput test can be rapidly executed.",
        NumberOfIterations = 3, RunMode = RunMode.Throughput,
        RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, ByteConstants.ThirtyTwoKb)]
        [GcTotalAssertion(GcMetric.TotalCollections, GcGeneration.Gen2, MustBe.ExactlyEqualTo, 0.0d)]
        public void Benchmark_GetUserDetails()
        {
            _response = user_Client.GetAsync(serviceBaseURL_User).Result;
            var responseResult =
                JsonConvert.DeserializeObject<List<User>>(_response.Content.ReadAsStringAsync().Result);
        }

        [PerfBenchmark(Description = "Test to ensure that a minimal throughput test can be rapidly executed.",
        NumberOfIterations = 3, RunMode = RunMode.Throughput,
        RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, ByteConstants.ThirtyTwoKb)]
        [GcTotalAssertion(GcMetric.TotalCollections, GcGeneration.Gen2, MustBe.ExactlyEqualTo, 0.0d)]
        public void Benchmark_GetProjectDetails()
        {
            _response = project_Client.GetAsync(serviceBaseURL_Project).Result;
            var responseResult =
                JsonConvert.DeserializeObject<List<view_ProjectSearch>>(_response.Content.ReadAsStringAsync().Result);
        }

        [PerfBenchmark(Description = "Test to ensure that a minimal throughput test can be rapidly executed.",
        NumberOfIterations = 3, RunMode = RunMode.Throughput,
        RunTimeMilliseconds = 1000, TestMode = TestMode.Test)]
        [MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, ByteConstants.ThirtyTwoKb)]
        [GcTotalAssertion(GcMetric.TotalCollections, GcGeneration.Gen2, MustBe.ExactlyEqualTo, 0.0d)]
        public void Benchmark_GetParentTaskDetails()
        {
            _response = parentTask_Client.GetAsync(serviceBaseURL_ParentTask).Result;
            var responseResult =
                JsonConvert.DeserializeObject<List<ParentTask>>(_response.Content.ReadAsStringAsync().Result);
        }
    }
}
