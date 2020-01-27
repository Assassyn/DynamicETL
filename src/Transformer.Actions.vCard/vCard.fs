namespace Transformer.Actions.vCard

open System.IO
open Transformer.Engine
open EWSoftware.PDI.Objects
open EWSoftware.PDI.Properties

module Reader =
  type VCardWriter(outputPath: string) =
    let createVCardFile (vcard: VCard) = 
      use stream = File.Open(outputPath, FileMode.Append)
      use writer = new StreamWriter(stream)
      vcard.WriteToStream(writer)
      writer.Close()
      stream.Close()

    let setPropertyWhenExists (properties: Map<string,string>) propertyname (setProperty: string -> unit) = 
      if properties.ContainsKey propertyname then setProperty properties.[propertyname] 

    interface IAction with
      member this.Execute entity = 
        let setProperty = setPropertyWhenExists entity.properties
        let vcard = VCard()

        setProperty "GivenName" (fun value -> vcard.Name.GivenName <- value)
        setProperty "FamilyName" (fun value -> vcard.Name.FamilyName <- value)
        setProperty "AdditionalNames" (fun value -> vcard.Name.AdditionalNames <- value)
        setProperty "NamePrefix" (fun value -> vcard.Name.NamePrefix <- value)
        setProperty "NameSuffix" (fun value -> vcard.Name.NameSuffix <- value)

        setProperty "E-mail 1 Address" (fun value -> vcard.EMailAddresses.Add(EMailTypes.Internet, value) |> ignore)
        setProperty "E-mail 2 Address" (fun value -> vcard.EMailAddresses.Add(EMailTypes.Internet, value) |> ignore)
        setProperty "E-mail 3 Address" (fun value -> vcard.EMailAddresses.Add(EMailTypes.Internet, value) |> ignore)

        setProperty "Home Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.Home, value) |> ignore)
        setProperty "Home Phone 2" (fun value -> vcard.Telephones.Add(PhoneTypes.Home, value) |> ignore)
        setProperty "Business Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.BBS, value) |> ignore)
        setProperty "Business Phone 2" (fun value -> vcard.Telephones.Add(PhoneTypes.BBS, value) |> ignore)
        setProperty "Mobile Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.Cell, value) |> ignore)
        setProperty "Car Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.Cell, value) |> ignore)
        setProperty "Other Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.None, value) |> ignore)
        setProperty "Primary Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.Preferred, value) |> ignore)
        setProperty "Pager" (fun value -> vcard.Telephones.Add(PhoneTypes.Pager, value) |> ignore)
        setProperty "Business Fax" (fun value -> vcard.Telephones.Add(PhoneTypes.Fax, value) |> ignore)
        setProperty "Home Fax" (fun value -> vcard.Telephones.Add(PhoneTypes.Fax, value) |> ignore)
        setProperty "Other Fax" (fun value -> vcard.Telephones.Add(PhoneTypes.Fax, value) |> ignore)
        setProperty "Company Main Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.BBS, value) |> ignore)
        setProperty "Callback" (fun value -> vcard.Telephones.Add(PhoneTypes.PCS, value) |> ignore)
        setProperty "Radio Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.Pager, value) |> ignore)
        setProperty "Telex" (fun value -> vcard.Telephones.Add(PhoneTypes.Text, value) |> ignore)
        setProperty "TTY/TDD Phone" (fun value -> vcard.Telephones.Add(PhoneTypes.TextPhone, value) |> ignore)

        createVCardFile vcard
        entity