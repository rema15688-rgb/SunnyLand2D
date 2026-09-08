# SunnyLand2D

مشروع Unity 2D جاهز للفتح في Unity 2022.1.24 — لعبة منصة بسيطة مع حركة وقفز، جمع عملات، 3 مراحل تُفتح تدريجياً، قائمة رئيسية، باوز، وموسيقى.

English & العربية: README يحتوي شرح التثبيت والتشغيل.

---

How to open and use in Unity (quick):
1. Clone repository or download ZIP.
2. Open Unity Hub -> Add -> select the repository folder.
3. Open project with Unity 2022.1.24.
4. Create scenes: MainMenu, Level1, Level2, Level3, GameOver, Win (if not present). Set them in Build Settings in this order (MainMenu = 0).
5. Create a GameManager empty object with GameManager, LevelManager, AudioManager, UIManager scripts. Assign UI Text to UIManager.
6. Create Player prefab: add Rigidbody2D, Collider2D, PlayerController, tag it "Player". Create a ground layer and set groundCheck Transform under player.
7. Create coin prefab: CircleCollider2D isTrigger, Coin script, assign collect sound.
8. Use placeholder art in Assets/Art and placeholder audio in Assets/Audio.

Files included:
- Assets/Scripts/*.cs — gameplay scripts
- Assets/Art/ (placeholder folder)
- Assets/Audio/ (placeholder folder)
- README.md (this file)

If you want, I can now prepare a ZIP release and attach it to the repository releases for direct download. Say "ZIP now" to proceed.
