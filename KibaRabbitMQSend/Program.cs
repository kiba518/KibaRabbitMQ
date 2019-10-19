using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KibaRabbitMQSend
{
    //rabbitmqctl stop_app 停止服务，会清除queue
    //rabbitmqctl start_app 开启服务
    //rabbitmqctl list_queues 查询当前队列
    //rabbitmqctl purge_queue kibaQueue 清空指定queue队列的数据
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//主机名，Rabbit会拿这个IP生成一个endpoint，这个很熟悉吧，就是socket绑定的那个终结点。
            factory.UserName = "guest";//默认用户名,用户可以在服务端自定义创建，有相关命令行
            factory.Password = "guest";//默认密码

            using (var connection = factory.CreateConnection())//连接服务器，即正在创建终结点。
            {
                //创建一个通道，这个就是Rabbit自己定义的规则了，如果自己写消息队列，这个就可以开脑洞设计了
                //这里Rabbit的玩法就是一个通道channel下包含多个队列Queue
                using (var channel = connection.CreateModel())
                {
                    for (int i = 0; i < 100; i++)
                    {
                        channel.QueueDeclare("kibaQueue", false, false, false, null);//创建一个名称为kibaQueue的消息队列
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 1;
                        string message = "I am Kiba518"; //传递的消息内容 
                        channel.BasicPublish("", "kibaQueue", properties, Encoding.UTF8.GetBytes(message)); //生产消息
                        Console.WriteLine($"Send:{message}");
                        Thread.Sleep(3000);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
