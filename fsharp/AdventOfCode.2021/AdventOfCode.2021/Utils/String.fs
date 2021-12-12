namespace Microsoft.FSharp.Core

open System

module String =
    let isUpper (s:string) =
        Seq.forall (fun c -> Char.IsUpper (c)) s

    let endsWith (value:string) (s:string) =
        s.EndsWith(value)

    let substring1 (index:int) (s:string) =
        s.Substring(index)

    let substring2 (index:int) (length:int) (s:string) =
        s.Substring(index,length)