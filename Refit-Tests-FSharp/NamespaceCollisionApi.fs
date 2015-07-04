namespace CollisionA
    
    type public SomeType = class end
 

namespace CollisionB
    
    type public SomeType = class end


namespace Refit.Tests.FSharp

    open System.Threading.Tasks
    type SomeType = CollisionA.SomeType
    open CollisionB
    open Refit


    type public INamespaceCollisionApi =

        [<Get("/")>]
        abstract member SomeRequest: unit -> Task<SomeType>


    type public NamespaceCollisionApi =

        static member Create() =
            RestService.For<INamespaceCollisionApi> "http://somewhere.com"
