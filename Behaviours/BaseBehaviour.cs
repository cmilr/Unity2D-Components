//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
	protected static bool _attackDisabled             = false;
	protected static float DIFFICULTY_DAMAGE_MODIFIER = 1f;

	// global direction
	protected const int RIGHT                         = 1;
	protected const int LEFT                          = -1;
	protected const int TOP                           = 2;
	protected const int BOTTOM                        = 3;
	protected const int UP                            = 4;
	protected const int DOWN                          = 5;
	protected const int ERROR                         = 0;
	protected const int NONE                          = 0;

	// global sizes
	protected const int SMALL                         = 1;
	protected const int MEDIUM                        = 2;
	protected const int LARGE                         = 3;
	protected const int XLARGE                        = 4;

	// global pauses
	protected const float WEAPON_PAUSE_ON_LEVEL_LOAD  = 1.5f;
	protected const float ENEMY_PAUSE_ON_LEVEL_LOAD   = 3f;

	// global measurements
	protected const float ONE_PIXEL                   = .03125f;
	protected const float ONE_HUD_PIXEL               = 5f;
	protected const float ONE_COLLIDER_PIXEL          = .62f;
	protected const float CULL_DISTANCE               = 20f;

	// gameObject names
	protected const string PLAYER                     = "Player";
	protected const string _PLAYER_DATA               = "_PlayerData";
	protected const string _GAME_DATA                 = "_GameData";
	protected const string GAME_STATE                 = "GameState";
	protected const string TILE_MAP                   = "TileMap";
	protected const string DEFAULT_TILE               = "DefaultTile";
	protected const string _OBJECT_POOL               = "_ObjectPool";

	// layer names
	protected const int PLAYER_LAYER                  = 8;
	protected const int PLAYER_COLLIDER               = 8;
	protected const int WEAPON_COLLIDER               = 9;
	protected const int BODY_COLLIDER                 = 10;
	protected const int ENEMY_COLLIDER                = 14;
	protected const int ENEMY_WEAPON                  = 17;
	protected const int BREAKABLES                    = 18;
	protected const int EDGE_BLOCKER                  = 24;
	protected const int PICKUP_LAYER                 = 15;

	// sorting layer names
	protected const string PLATFORM_SORTING_LAYER      = "Platforms";
	protected const string INTERACTIVE_SORTING_LAYER   = "Interactives";
	protected const string PICKUP_SORTING_LAYER        = "Pickups";
	protected const string ENEMY_SORTING_LAYER         = "Enemies";
	protected const string HERO_ARM_SORTING_LAYER      = "HeroArm";
	protected const string HERO_WEAPON_SORTING_LAYER   = "HeroWeapon";
	protected const string HERO_SORTING_LAYER          = "Hero";
	protected const string FOREGROUND_SORTING_LAYER    = "Foreground";
	protected const string PROJECTILE_SORTING_LAYER    = "Projectiles";

	// player offsets
	protected const float ABOUTFACE_OFFSET            = 0f;

	// player/weapon animation speeds
	protected const float IDLE_SPEED                  = 1f;
	protected const float RUN_SPEED                   = .5f;
	protected const float JUMP_SPEED                  = 10f;
	protected const float SWING_SPEED                 = 1f;
	protected const float HURL_SPEED                  = 1f;

	// player/weapon states
	protected const int IDLE                          = 0;
	protected const int RUN                           = 1;
	protected const int JUMP                          = 2;
	protected const int FALL                          = 3;
	protected const int ATTACK                        = 4;
	protected const int RUN_ATTACK                    = 5;
	protected const int JUMP_ATTACK                   = 6;

	// weapon types
	protected const int MELEE                         = 0;
	protected const int PROJECTILE                    = 1;
	protected const int SWORD                         = 3;
	protected const int AXE                           = 4;
	protected const int HAMMER                        = 5;

	// player/death animation speeds
	protected const float STRUCKDOWN_SPEED            = 1f;
	protected const float DROWNED_SPEED               = 8f;

	// entity auto-alignment settings
	protected const float ALIGN_ENTITY_TO             = .124f;

	// hud specs
	protected const float HUD_WEAPON_TOP_MARGIN       = 125f;
	protected const float HUD_STASHED_WEAPON_OFFSET   = 85f;
	protected const float HUD_STASHED_TRANSPARENCY    = .35f;
	protected const float HUD_HEARTS_TOP_MARGIN       = 17f;
	protected const float HUD_Z                       = 10f;
	protected const float HUD_FADE_IN_AFTER           = .75f;
	protected const float HUD_FADE_OUT_AFTER          = .25f;
	protected const float HUD_INITIAL_TIME_TO_FADE    = 2f;
	protected const float HUD_WEAPON_CHANGE_FADE      = 0f;
	protected const float SPACE_BETWEEN_WEAPONS       = 1.3281f;
	protected const float INVENTORY_SHIFT_SPEED       = .15f;

	// breakable pieces
	protected const int MIN_BEFORE_FADE               = 5;
	protected const int MAX_BEFORE_FADE               = 10;
}
