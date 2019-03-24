namespace BusinessLogic.Dtos.Interfaces
{
    interface IDtoEvent<TDto> where TDto : class
    {
        void BeforeGet(TDto dto);
        void BeforeInsert(TDto dto);
        void AfterInsert(TDto dto);
        void BeforeUpdate(TDto dto);
        void AfterUpdate(TDto dto);
        void BeforeSave(TDto dto);
        void AfterSave(TDto dto);
    }
}
