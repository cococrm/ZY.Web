namespace ZY.Core.Entities
{
    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// 实体主键标识
        /// </summary>
        TKey Id { get; set; }
    }
}
