﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.ServerObjects
{
    class ClientConnection
    {
        private TcpClient _controlClient;

        private NetworkStream _controlStream;
        private StreamReader _controlReader;
        private StreamWriter _controlWriter;

        private string _username;
        public ClientConnection(TcpClient client)
        {
            _controlClient = client;
            _controlStream = _controlClient.GetStream();
            _controlReader = new StreamReader(_controlStream, Encoding.ASCII);
            _controlWriter = new StreamWriter(_controlStream, Encoding.ASCII);
        }

        public void HandleClient(object obj)
        {
            _controlWriter.WriteLine("220 Service Ready.");
            _controlWriter.Flush();

            string line;

            try
            {
                while (!string.IsNullOrEmpty(line = _controlReader.ReadLine()))
                {
                    string response = null;
                    string[] command = line.Split(' ');
                    string cmd = command[0].ToUpperInvariant();
                    string arguments = command.Length > 1 ? line.Substring(cmd.Length + 1) : null;

                    if (string.IsNullOrWhiteSpace(arguments))
                        arguments = null;

                    if (response == null)
                    {
                        switch (cmd)
                        {
                            case "USER":
                                response = User(arguments);
                                break;
                            case "PASS":
                                response = Password(arguments);
                                break;
                            case "CWD":
                                response = ChangeWorkingDirectory(arguments);
                                break;
                            case "CDUP":
                                response = ChangeWorkingDirectory("..");
                                break;
                            case "PWD":
                                response = "257 \"/\" is current directory.";
                                break;
                            case "QUIT":
                                response = "221 Service closing control connection";
                                break;
                            case "TYPE":
                                string[] splitArgs = arguments.Split(' ');
                                response = Type(splitArgs[0], splitArgs.Length > 1 ? splitArgs[1] : null);
                                break;

                            default:
                                response = "502 Command not implemented";
                                break;
                        }
                    }

                    if (_controlClient == null || !_controlClient.Connected)
                    {
                        break;
                    }
                    else
                    {
                        _controlWriter.WriteLine(response);
                        _controlWriter.Flush();

                        if (response.StartsWith("221"))
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #region FTP Commands

        private string User(string username)
        {
            _username = username;

            return "331 Username ok, need password";
        }

        private string Password(string password)
        {
            if (true)
                return "230 User logged in";
            else
                return "530 Not logged in";
        }

        private string ChangeWorkingDirectory(string pathname)
        {
            return "250 Changed to new directory";
        }

        private string Type(string type_code, string format_control)
        {
            string response = "";
            Console.WriteLine(type_code);
            switch (type_code)
            {
                case "A":
                case "I":
                    response = "200 OK";
                    Console.WriteLine("ok");
                    break;
                case "E":
                case "L":
                default:
                    response = "504 Command not implemented for that parameter.";
                    break;
            }

            switch (format_control)
            {
                case "N":
                case "T":
                case "C":
                default:
                    response = "200 OK";
                    break;
            }

            return response;
        }

        #endregion
    }
}
