namespace AddressBook.CoreLibrary;
public static class ParserClass
{
    public static async Task<BasicList<AddressModel>> DownloadAddressBookFromPathAsync(string directoryPath)
    {
        string content = await fs1.AllTextAsync(directoryPath);
        return DownloadAddressBookFromContent(content);
    }
    public static BasicList<AddressModel> DownloadAddressBookFromContent(string content)
    {
        //the format is a standard format so can create a nuget package out of this.
        HtmlParser parses = new();
        parses.Body = content;
        BasicList<AddressModel> output = new();
        BasicList<string> list = parses.GetList("BEGIN:VCARD", "END:VCARD");
        foreach (var item in list)
        {
            parses.Body = item;
            string fullName = parses.GetSomeInfo("FN:", $"{Constants.VBCrLf}GENDER:");
            if (fullName.Contains('*') == false)
            {
                string startAddress = parses.GetSomeInfo("ADR;HOME;", $"{Constants.VBCrLf}REV:");
                AddressModel address = new();
                address.FullName = fullName;
                BasicList<string> temp = startAddress.Split(";").ToBasicList();
                address.Street2 = temp[1];
                address.Street1 = temp[2];
                address.City = temp[3];
                address.State = temp[4];
                address.PostalCode = temp[5];
                output.Add(address);
            }
        }
        return output;
    }
}