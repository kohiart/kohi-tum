// Copyright (c) Joshua Davis / praystation. All rights reserved. You do not have permission to reproduce, modify, or redistribute this code or its outputs without express permission from the artist.

using Kohi.Composer;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Text.Json;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace UniverseMachine.Renderer
{
    internal sealed class RenderEthereum
    {
        private static Task _backgroundTask = null!;

        private static readonly string RpcUrl;

        static RenderEthereum()
        {
            RpcUrl = "http://localhost:8545";
        }

        public static async Task<string> ExecuteAsync(string manifestPath, string folder, BigInteger token, int seed)
        {
            ClientBase.ConnectionTimeout = TimeSpan.FromHours(500);

            var cancellationSource = new CancellationTokenSource();
            var cancellationToken = cancellationSource.Token;

            _backgroundTask = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, cancellationToken);
                    GetVmMemory();
                }
            }, cancellationToken);

            var startMemory = GetVmMemory().GetValueOrDefault();

            var http = new HttpClient();
            http.Timeout = TimeSpan.FromHours(500);
            http.MaxResponseContentBufferSize = 2147483647;

            var rpc = new RpcClient(new Uri(RpcUrl, UriKind.Absolute), http);
            var account = new Account("0xb0a587bc9681a7333763f84c3b90a4d58bd01b5fb0635ac16187f6f55e792a57", 8134646);

            string? filename = null;
            var sw = Stopwatch.StartNew();
            try
            {
                var manifestJson = await File.ReadAllTextAsync(manifestPath, cancellationToken);
                var manifest = JsonSerializer.Deserialize<Manifest>(manifestJson);
                if (manifest == null || string.IsNullOrWhiteSpace(manifest.UniverseMachineParameters) ||
                    string.IsNullOrWhiteSpace(manifest.UniverseMachineRenderer))
                    throw new InvalidOperationException("Missing manifest!");

                var web3 = new Web3(account, rpc);
                var contractHandler = web3.Eth.GetContractHandler(manifest.UniverseMachineRenderer);
                var output = await RenderQueryAsync(contractHandler, token, seed, manifest.UniverseMachineParameters);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract (it really does return null)
                if (output == null)
                {
                    await Console.Error.WriteLineAsync(
                        "Output buffer was empty, which usually means early exit from the eth_call");
                    return null!;
                }

                var empty = output.Aggregate(true, (current, pixel) => current & (pixel == 0));
                if (empty)
                    await Console.Error.WriteLineAsync("buffer is empty!");
                else
                    await Console.Out.WriteLineAsync("buffer has data!");

                var g = new Graphics2D(1024, 1280) { Buffer = output.ToArray() };
                filename = Path.Combine(folder, $"TUM_Solidity_{token}_{seed}.png");
                ImageData.Save(filename, g);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Rendering failed after {sw.Elapsed}");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());

                var sb = new StringBuilder();
                sb.AppendLine(e.ToString());

                var endMemory = GetVmMemory().GetValueOrDefault();
                sb.AppendLine($"Used RAM = {(endMemory - startMemory) / 1024 / 1024}MB");

                await File.WriteAllTextAsync("log.txt", sb.ToString(), cancellationToken);
            }
            finally
            {
                Console.WriteLine($"Completed, took {sw.Elapsed}");
                Console.WriteLine();
                var endMemory = GetVmMemory().GetValueOrDefault();
                Console.WriteLine($"Used RAM = {(endMemory - startMemory) / 1024 / 1024}MB");
            }

            try
            {
                cancellationSource.Cancel();
                await _backgroundTask.WaitAsync(cancellationToken);
            }
            catch
            {
                // ignored
            }

            return filename!;
        }

        public static Task<List<byte>> RenderQueryAsync(ContractHandler contractHandler, BigInteger tokenId, int seed,
            string parameters)
        {
            return contractHandler.QueryAsync<RenderQueryFunction, List<byte>>(new RenderQueryFunction
            {
                TokenId = tokenId,
                Seed = seed,
                Parameters = parameters
            });
        }

        private static long? GetVmMemory()
        {
            var process = TryGetDockerProcess();
            Console.WriteLine($"VM RAM: {process?.WorkingSet64 / 1024 / 1024}MB");
            return process?.WorkingSet64;
        }

        private static Process? TryGetDockerProcess()
        {
            return Process.GetProcessesByName("Vmmem").FirstOrDefault() ??
                   Process.GetProcessesByName("VmmemWSL").FirstOrDefault();
        }

        [Function("render", "uint8[]")]
        public class RenderQueryFunction : FunctionMessage
        {
            [Parameter("uint256", "tokenId")] public virtual BigInteger TokenId { get; set; }

            [Parameter("int32", "seed", 2)] public virtual int Seed { get; set; }

            [Parameter("address", "parameters", 3)]
            public virtual string Parameters { get; set; } = null!;
        }
    }
}
