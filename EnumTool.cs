using System.ComponentModel;
using System.Reflection;

namespace Codeove.MvcTool;

public static class EnumTool
{
    /// <summary>
    /// Enum değerini KeyValuePair<byte, string> olarak döndürür
    /// </summary>
    /// <param name="enumValue">Enum değeri</param>
    /// <returns>KeyValuePair<byte, string> (Id, Value)</returns>
    public static KeyValuePair<byte, string> ToKeyValuePair(this Enum enumValue)
    {
        if (enumValue == null)
            throw new ArgumentNullException(nameof(enumValue));

        byte id = Convert.ToByte(enumValue); // byte'a çeviriyoruz
        string value = GetEnumDescription(enumValue);

        return new KeyValuePair<byte, string>(id, value);
    }

    /// <summary>
    /// Enum tipindeki tüm değerleri KeyValuePair<byte, string> listesi olarak döndürür
    /// </summary>
    /// <typeparam name="T">Enum tipi</typeparam>
    /// <returns>KeyValuePair<byte, string> listesi</returns>
    public static IEnumerable<KeyValuePair<byte, string>> GetAllKeyValuePairs<T>() where T : Enum
    {
        var type = typeof(T);

        if (!type.IsEnum)
            throw new ArgumentException("T must be an enumerated type");

        return Enum.GetValues(type)
                   .Cast<T>()
                   .Select(e => new KeyValuePair<byte, string>(
                       Convert.ToByte(e),
                       GetEnumDescription(e)));
    }

    /// <summary>
    /// Enum tipindeki tüm değerleri Dictionary<byte, string> olarak döndürür
    /// </summary>
    /// <typeparam name="T">Enum tipi</typeparam>
    /// <returns>Dictionary<byte, string></returns>
    public static Dictionary<byte, string> ToByteDictionary<T>() where T : Enum
    {
        return GetAllKeyValuePairs<T>().ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Enum tipindeki tüm değerleri IDictionary<byte, string> olarak döndürür
    /// </summary>
    /// <typeparam name="T">Enum tipi</typeparam>
    /// <returns>IDictionary<byte, string></returns>
    public static IDictionary<byte, string> ToByteIDictionary<T>() where T : Enum
    {
        return ToByteDictionary<T>();
    }

    /// <summary>
    /// Enum değerinin Description attribute'undan açıklamasını alır
    /// </summary>
    private static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// Byte değerinden enum'a çevirir ve KeyValuePair döndürür
    /// </summary>
    /// <typeparam name="T">Enum tipi</typeparam>
    /// <param name="id">Byte ID değeri</param>
    /// <returns>KeyValuePair<byte, string></returns>
    public static KeyValuePair<byte, string> FromByte<T>(byte id) where T : Enum
    {
        if (Enum.IsDefined(typeof(T), id))
        {
            var enumValue = (T)Enum.ToObject(typeof(T), id);
            return new KeyValuePair<byte, string>(id, GetEnumDescription(enumValue));
        }

        throw new ArgumentException($"Id {id} is not defined in enum {typeof(T).Name}");
    }

    /// <summary>
    /// Byte dizisinden KeyValuePair listesi oluşturur
    /// </summary>
    /// <typeparam name="T">Enum tipi</typeparam>
    /// <param name="ids">Byte ID'leri</param>
    /// <returns>KeyValuePair<byte, string> listesi</returns>
    public static IEnumerable<KeyValuePair<byte, string>> FromBytes<T>(params byte[] ids) where T : Enum
    {
        foreach (byte id in ids)
        {
            if (Enum.IsDefined(typeof(T), id))
            {
                var enumValue = (T)Enum.ToObject(typeof(T), id);
                yield return new KeyValuePair<byte, string>(id, GetEnumDescription(enumValue));
            }
        }
    }

    /// <summary>
    /// Enum değerlerini IReadOnlyDictionary olarak döndürür
    /// </summary>
    public static IReadOnlyDictionary<byte, string> ToReadOnlyDictionary<T>() where T : Enum
    {
        return new System.Collections.ObjectModel.ReadOnlyDictionary<byte, string>(
            ToByteDictionary<T>());
    }

    /// <summary>
    /// Sadece değerleri (açıklamaları) listeler
    /// </summary>
    public static IEnumerable<string> GetValues<T>() where T : Enum
    {
        return GetAllKeyValuePairs<T>().Select(kvp => kvp.Value);
    }

    /// <summary>
    /// Sadece ID'leri (byte) listeler
    /// </summary>
    public static IEnumerable<byte> GetKeys<T>() where T : Enum
    {
        return GetAllKeyValuePairs<T>().Select(kvp => kvp.Key);
    }

    /// <summary>
    /// Value'dan Key bulma
    /// </summary>
    public static byte? GetKeyByValue<T>(string value) where T : Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var kvp = GetAllKeyValuePairs<T>()
            .FirstOrDefault(x => x.Value.Equals(value, StringComparison.OrdinalIgnoreCase));

        return kvp.Key;
    }
}