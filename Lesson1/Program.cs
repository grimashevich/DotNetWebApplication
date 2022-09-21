using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lesson1
{
	internal class Program
	{
		static readonly HttpClient client = new HttpClient();

		static void Main()
		{
			List<Task<Post>> taskList = new List<Task<Post>>();
			
			
			for (int i = 4; i < 14; i++)
			{
				taskList.Add(GetPostAsync(i));
			}
			Task.WhenAll(taskList).Wait();
			Task.WaitAll(WriteToFile(taskList));
		}

		static async Task<Post?> GetPostAsync(int id)
		{
			Post? post;
			
			try
			{
				Console.WriteLine($"Task id {id} start...");
				string responseBody = await client.GetStringAsync($"https://jsonplaceholder.typicode.com/posts/{id}");
				post = JsonSerializer.Deserialize<Post>(responseBody);
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine("\nException Caught!");
				Console.WriteLine("Message :{0} ", e.Message);
				post = null;
			}
			Console.WriteLine($"Task id {id} end...");
			return post;
		}

		static async Task WriteToFile(List<Task<Post>> taskList)
		{
			StringBuilder result = new StringBuilder();

			foreach (var task in taskList)
				result.Append(task.Result);
			await File.WriteAllTextAsync("result.txt", result.ToString());
		}
	}
}