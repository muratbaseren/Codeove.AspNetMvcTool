# Codeove ASPNET Core MVC Tools
ASP.NET Core MVC projelerinde kullanýlmak üzere çeþitli yardýmcý metotlar ve geniþletmeler içeren bir kütüphane.

- EnumTool
- TempData Extensions

## EnumTool
Bu kütüphane, enum tipleri ile çalýþmayý kolaylaþtýran geniþletme metotlarý saðlar. Enum deðerlerini KeyValuePair olarak alma, tüm enum deðerlerini listeleme, byte türünde dictionary oluþturma ve byte'dan enum deðeri bulma gibi iþlevler sunar. Ayrýca, LINQ sorgularý ve JSON serileþtirme iþlemleri için de kullanýlabilir.

```csharp
using System.ComponentModel;

// Enum tanýmý
public enum UserStatus : byte // byte olarak tanýmlayabiliriz
{
    [Description("Aktif Kullanýcý")]
    Active = 1,
    
    [Description("Pasif Kullanýcý")]
    Inactive = 2,
    
    [Description("Askýya Alýnmýþ")]
    Suspended = 3
}

public enum PermissionLevel : byte
{
    [Description("Okuma")]
    Read = 10,
    
    [Description("Yazma")]
    Write = 20,
    
    [Description("Yönetici")]
    Admin = 30
}

class Program
{
    static void Main()
    {
        // Tekil enum deðerini KeyValuePair olarak alma
        var status = UserStatus.Active;
        var keyValuePair = status.ToKeyValuePair();
        Console.WriteLine($"Key: {keyValuePair.Key}, Value: {keyValuePair.Value}");
        // Çýktý: Key: 1, Value: Aktif Kullanýcý

        // Tüm enum deðerlerini listeleme
        var allValues = EnumExtensions.GetAllKeyValuePairs<UserStatus>();
        foreach (var kvp in allValues)
        {
            Console.WriteLine($"ID: {kvp.Key}, Açýklama: {kvp.Value}");
        }
        // Çýktý:
        // ID: 1, Açýklama: Aktif Kullanýcý
        // ID: 2, Açýklama: Pasif Kullanýcý
        // ID: 3, Açýklama: Askýya Alýnmýþ

        // Dictionary olarak alma
        var dict = EnumExtensions.ToByteDictionary<UserStatus>();
        Console.WriteLine($"Dictionary Count: {dict.Count}"); // 3

        // Byte'dan enum deðeri bulma
        var fromByte = EnumExtensions.FromByte<UserStatus>(2);
        Console.WriteLine($"From byte 2: Key: {fromByte.Key}, Value: {fromByte.Value}");
        // Çýktý: Key: 2, Value: Pasif Kullanýcý

        // Birden fazla byte'dan deðer bulma
        var multiple = EnumExtensions.FromBytes<UserStatus>(1, 3).ToList();
        foreach (var item in multiple)
        {
            Console.WriteLine($"Multiple: {item.Key} - {item.Value}");
        }

        // LINQ ile kullaným
        var activeItems = allValues.Where(kvp => kvp.Value.Contains("Aktif"));
        var firstItem = allValues.FirstOrDefault(kvp => kvp.Key == 1);
        
        // JSON serialization için kullaným
        var jsonReadyData = dict.Select(kvp => new 
        { 
            Id = kvp.Key, 
            Name = kvp.Value 
        }).ToList();

        // Farklý enum tipi ile kullaným
        var permissions = EnumExtensions.GetAllKeyValuePairs<PermissionLevel>();
        foreach (var perm in permissions)
        {
            Console.WriteLine($"Permission: {perm.Key} = {perm.Value}");
        }
        // Çýktý:
        // Permission: 10 = Okuma
        // Permission: 20 = Yazma
        // Permission: 30 = Yönetici
    }
}
```

## TempData Extensions
ASP.NET Core MVC projelerinde TempData ile çalýþmayý kolaylaþtýran geniþletme metotlarý saðlar. Nesneleri JSON formatýnda TempData'ya kaydetme ve okuma iþlemlerini basitleþtirir.

```csharp
TempData.Put("Message", new Message { Text = "OK", Type = "success" });
var msg = TempData.Get<Message>("Message");
```