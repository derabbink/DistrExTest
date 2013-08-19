# DistrExTest

Demo app demonstrating [DistrEx][distrex] features


## Getting started

Create a folder named `lib\` in the [DistrExTest][DistrExTest] solution folder. 
You'll need to obtain a compiled version of [DistrEx][distrex].

Now add the following DLLs to `lib\` manually (other dependencies will be added by NuGet):
* DistrEx.PlugIn.dll
* DistrEx.Coordinator.dll
* DistrEx.Coordinator.Interface.dll
* DistrEx.Communication.Service.dll
* DistrEx.Communication.Proxy.dll
* DistrEx.Communication.Contracts.dll
* DistrEx.Common.dll
* TestApiWpf.dll (download from [here][testapi])

From you compiled DistrEx, copy *all* the DLLs from the [DistrEx.Worker.Host][worker-host] project twice: once to each of the following directories:
[`workers/PingWorker/`][pingworker] and [`workers/PongWorker/`][pongworker].
*Both* these folders should now contain the following DLLs
* DistrEx.Worker.dll 
* DistrEx.Communication.Contracts.dll
* DistrEx.Communication.Services.dll 
* DistrEx.Common.dll 
* DistrEx.PlugIn.dll 
* DistrEx.Worker.Host.exe
* System.Reactive.Linq.dll
* Sytsem.Reactive.Core.dll
* System.Reactive.Intefaces.dll
* DependencyResolver.dll

[distrex]: https://github.com/derabbink/DistrEx
[worker-host]: https://github.com/derabbink/DistrEx/tree/master/DistrEx.Worker.Host
[pingworker]: https://github.com/derabbink/DistrExTest/tree/master/workers/PingWorker
[pongworker]: https://github.com/derabbink/DistrExTest/tree/master/workers/PongWorker
[DistrExTest]: https://github.com/derabbink/DistrExTest/tree/master
[testapi]: http://testapi.codeplex.com/
