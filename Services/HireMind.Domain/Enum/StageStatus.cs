namespace HireMind.Domain.Enum;

public enum StageStatus
{
    New,          // الطلب وصل (لـ Initiate Application)
    Selected,     // تم اختيار المتقدم للمرحلة التالية
    NotSelected,  // تم رفض المتقدم
    Approved      // المرحلة النهائية تم اجتيازها بنجاح
}