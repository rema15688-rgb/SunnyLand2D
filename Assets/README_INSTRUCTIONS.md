# Instructions (العربية)

1. افتح Unity Hub وأضف مجلد المشروع.
2. افتح المشروع باستخدام Unity 2022.1.24.
3. أنشئ المشاهد التالية وقم بإضافتها إلى Build Settings بالترتيب: MainMenu (0), Level1 (1), Level2 (2), Level3 (3), GameOver (4), Win (5).
4. أضف كائن فارغ يسمى Managers وضع عليه سكربتات: GameManager, LevelManager, AudioManager, UIManager.
5. أنشئ لاعب (Player) مع Rigidbody2D و Collider2D وجسم فرعي GroundCheck (Transform) واضبط Layer "Ground" للأرض.
6. ضع Coins في المشهد كـ prefab مع Collider2D تفعيل IsTrigger و سكربت Coin.
7. اضبط واجهة المستخدم لعرض العملات باستخدام UI Text واربطه في UIManager.
8. لإضافة موسيقى أو مؤثرات ضع الملفات داخل Assets/Audio وقم بربطها في AudioManager أو في Coin.collectSound.

إذا أردت ZIP الآن اكتب: "ZIP now"
