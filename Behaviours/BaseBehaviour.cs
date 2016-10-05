using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
	//global direction
	protected const int RIGHT = 1;
	protected const int LEFT = -1;
	protected const int TOP = 2;
	protected const int BOTTOM = 3;
	protected const int UP = 4;
	protected const int DOWN = 5;
	protected const int CENTER = 6;
	protected const int TOP_LEFT = 7;
	protected const int TOP_CENTER = 8;
	protected const int TOP_RIGHT = 9;
	protected const int MIDDLE_LEFT = 10;
	protected const int MIDDLE_CENTER = 11;
	protected const int MIDDLE_RIGHT = 12;
	protected const int BOTTOM_LEFT = 13;
	protected const int BOTTOM_CENTER = 14;
	protected const int BOTTOM_RIGHT = 15;
	protected const int ERROR = 0;
	protected const int NONE = 0;

	//global sizes
	protected const int SMALL = 1;
	protected const int MEDIUM = 2;
	protected const int LARGE = 3;
	protected const int XLARGE = 4;

	//global difficulty
	protected const int NORMAL = 1;
	protected const int HARD = 2;
	protected const int KILL_ME_NOW = 3;

	//global pauses
	protected const float WEAPON_PAUSE_ON_LEVEL_LOAD = 1.5f;
	protected const float ENEMY_PAUSE_ON_LEVEL_LOAD = 3f;

	//global measurements
	protected const float ONE_PIXEL = .03125f;
	protected const float ONE_COLLIDER_PIXEL = .62f;
	protected const float CULL_DISTANCE = 20f;

	//gameObject names
	protected const string _DATA = "_Data";
	protected const string PLAYER = "Player";
	protected const string TILE_MAP = "TileMap";
	protected const string GAME_MANAGER = "GameManager";

	//layer names
	protected const int PLAYER_DEFAULT_LAYER = 8;
	protected const int PLAYER_PHYSICS_LAYER = 8;
	protected const int PLAYER_BODY_COLLIDER = 10;
	protected const int PLAYER_WEAPON_COLLIDER = 9;
	protected const int ENEMY_BODY_COLLIDER = 14;
	protected const int ENEMY_WEAPON_COLLIDER = 17;
	protected const int BREAKABLES = 18;
	protected const int EDGE_BLOCKER = 24;
	protected const int PICKUP_LAYER = 15;
	protected const int PICKUP_PHYSICS_LAYER = 16;
	protected const int PLATFORM_LAYER = 21;
	protected const int NO_COLLISION_LAYER = 30;

	//sorting layer names
	protected const string PLATFORM_SORTING_LAYER = "Platforms";
	protected const string INTERACTIVE_SORTING_LAYER = "Interactives";
	protected const string PICKUP_SORTING_LAYER = "Pickups";
	protected const string ENEMY_SORTING_LAYER = "Enemies";
	protected const string HERO_ARM_SORTING_LAYER = "HeroArm";
	protected const string HERO_WEAPON_SORTING_LAYER = "HeroWeapon";
	protected const string HERO_SORTING_LAYER = "Hero";
	protected const string FOREGROUND_SORTING_LAYER = "Foreground";
	protected const string PROJECTILE_SORTING_LAYER = "Projectiles";

	// player actions
	protected const int IDLE = 0;
	protected const int RUN = 1;
	protected const int JUMP = 2;
	protected const int ATTACK = 3;
	protected const int RUN_ATTACK = 4;
	protected const int JUMP_ATTACK = 5;

	// player/weapon animation speeds
	protected const float IDLE_SPEED = 1f;
	protected const float RUN_SPEED = .5f;
	protected const float JUMP_SPEED = 8f;
	protected const float ATTACK_SPEED = 1.2f;
	protected const float HURL_SPEED = 1f;

	//player offsets
	protected const float ABOUTFACE_OFFSET = .25f;

	//weapon types
	protected const int MELEE = 0;
	protected const int PROJECTILE = 1;

	//player/death animation speeds
	protected const float STRUCKDOWN_SPEED = 1f;
	protected const float DROWNED_SPEED = 8f;

	//entity auto-alignment settings
	protected const float ALIGN_ENTITY_TO = .124f;

	//breakable pieces
	protected const int MIN_BEFORE_FADE = 5;
	protected const int MAX_BEFORE_FADE = 10;

#if UNITY_STANDALONE_OSX

	// CAMERA & HUD CANVAS SIZE
	protected const float PLATFORM_SPECIFIC_ORTHOGRAPHIC_FACTOR = 128f;  // 32, 48, 64, 96, etc.
	protected const float PLATFORM_SPECIFIC_CANVAS_SCALE = 1f;

	// HUD
	protected const float HUD_Z = 10f;
	protected const float HUD_FADE_IN_AFTER = .75f;
	protected const float HUD_FADE_OUT_AFTER = .25f;
	protected const float HUD_INITIAL_TIME_TO_FADE = 2f;

	// TEXT-BASED HUD ELEMENTS
	
	// HUD ITEM TITLE
	protected const int ITEM_TITLE_ALIGNMENT = TOP_CENTER;
	protected const float ITEM_TITLE_X_POS = 0f;
	protected const float ITEM_TITLE_Y_POS = 20f;
	
	// HUD XP
	protected const int XP_ALIGNMENT = TOP_LEFT;
	protected const float XP_X_POS = 40f;
	protected const float XP_Y_POS = 100f;

	// HUD LEVEL
	protected const int LEVEL_ALIGNMENT = TOP_LEFT;
	protected const float LEVEL_X_POS = 40f;
	protected const float LEVEL_Y_POS = 150f;

	// HUD HP
	protected const int HP_ALIGNMENT = TOP_RIGHT;
	protected const float HP_X_POS = 40f;
	protected const float HP_Y_POS = 150f;

	// HUD AC
	protected const int AC_ALIGNMENT = TOP_RIGHT;
	protected const float AC_X_POS = 40f;
	protected const float AC_Y_POS = 200f;

	// HUD SCORE
	protected const int SCORE_ALIGNMENT = BOTTOM_LEFT;
	protected const float SCORE_X_POS = 0f;
	protected const float SCORE_Y_POS = 0f;

	// SPRITE-BASED HUD ELEMENTS
	
	// HUD INVENTORY
	protected const float INVENTORY_Y_POS = 200f;
	protected const float STASHED_ITEM_OFFSET = 150f;
	protected const float STASHED_ITEM_TRANSPARENCY = .35f;
	protected const float DISTANCE_TO_SLIDE_ITEMS = 1.3281f;
	protected const float INVENTORY_SHIFT_SPEED = .15f;
	protected const float ITEM_CHANGE_FADE = 0f;

	// HUD HEARTS
	protected const int HEART_ALIGNMENT = TOP_RIGHT;
	protected const float HEART_X_POS = 40f;
	protected const float HEART_Y_POS = 80f;
	protected const float HEART_OFFSET = 60f;

	// HUD WATER DROP
	protected const int WATER_DROP_ALIGNMENT = TOP_RIGHT;
	protected const float WATER_DROP_X_POS = 100f;
	protected const float WATER_DROP_Y_POS = 80f;

	// CAMERA
	protected const float MIN_TOP_SCREEN_MARGIN = 3f;
	protected const float MIN_BOTTOM_SCREEN_MARGIN = 1f;
	protected const float PLAYER_X_MOVEMENT_BEFORE_CAM_FOLLOWS = .5f;
	protected const float CAM_X_SPEED_TO_FOLLOW = 7f;                    // how quickly camera catches up with its target movement in the x axis.
	protected const float CAM_Y_SPEED_TO_FOLLOW = 5f;                    // how quickly camera catches up with its target movement in the y axis.
	protected const float CAM_STARTING_X_POSITION = 0f;                  // starting position, from bottom left corner, in relation to player.
	protected const float CAM_STARTING_Y_POSITION = 0f;                  // starting position, from bottom left corner, in relation to player.
	
	// DEBUG FPS COUNTER
	protected const int FPS_ALIGNMENT = TOP_RIGHT;
	protected const float FPS_X_POS = 40f;
	protected const float FPS_Y_POS = 50f;

#endif

#if UNITY_IOS

	// CAMERA & HUD CANVAS SIZE
	//protected const float PLATFORM_SPECIFIC_ORTHOGRAPHIC_FACTOR = 256f;  // 32, 48, 64, 96, etc.
	//protected const float PLATFORM_SPECIFIC_CANVAS_SCALE = 2f;

	protected const float PLATFORM_SPECIFIC_ORTHOGRAPHIC_FACTOR = 224f;  // 32, 48, 64, 96, etc.
	protected const float PLATFORM_SPECIFIC_CANVAS_SCALE = 1.75f;

	// HUD
	protected const float HUD_Z = 10f;
	protected const float HUD_FADE_IN_AFTER = .75f;
	protected const float HUD_FADE_OUT_AFTER = .25f;
	protected const float HUD_INITIAL_TIME_TO_FADE = 2f;

	// TEXT-BASED HUD ELEMENTS

	// HUD ITEM TITLE
	protected const int ITEM_TITLE_ALIGNMENT = TOP_CENTER;
	protected const float ITEM_TITLE_X_POS = 0f;
	protected const float ITEM_TITLE_Y_POS = 20f;

	// HUD XP
	protected const int XP_ALIGNMENT = TOP_LEFT;
	protected const float XP_X_POS = 40f;
	protected const float XP_Y_POS = 100f;

	// HUD LEVEL
	protected const int LEVEL_ALIGNMENT = TOP_LEFT;
	protected const float LEVEL_X_POS = 40f;
	protected const float LEVEL_Y_POS = 150f;

	// HUD HP
	protected const int HP_ALIGNMENT = TOP_RIGHT;
	protected const float HP_X_POS = 40f;
	protected const float HP_Y_POS = 150f;

	// HUD AC
	protected const int AC_ALIGNMENT = TOP_RIGHT;
	protected const float AC_X_POS = 40f;
	protected const float AC_Y_POS = 200f;

	// HUD SCORE
	protected const int SCORE_ALIGNMENT = TOP_LEFT;
	protected const float SCORE_X_POS = 40f;
	protected const float SCORE_Y_POS = 86.5f;

	// SPRITE-BASED HUD ELEMENTS

	// HUD INVENTORY
	protected const float INVENTORY_Y_POS = 350f;
	protected const float STASHED_ITEM_OFFSET = 250f;
	protected const float STASHED_ITEM_TRANSPARENCY = .35f;
	protected const float DISTANCE_TO_SLIDE_ITEMS = 1.3281f;
	protected const float INVENTORY_SHIFT_SPEED = .15f;
	protected const float ITEM_CHANGE_FADE = 0f;

	// HUD HEARTS
	protected const int HEART_ALIGNMENT = TOP_RIGHT;
	protected const float HEART_X_POS = 40f;
	protected const float HEART_Y_POS = 85f;
	protected const float HEART_OFFSET = 60f;

	// HUD WATER DROP
	protected const int WATER_DROP_ALIGNMENT = TOP_RIGHT;
	protected const float WATER_DROP_X_POS = 100f;
	protected const float WATER_DROP_Y_POS = 85f;

	// CAMERA
	protected const float MIN_TOP_SCREEN_MARGIN = 3.25f;
	protected const float MIN_BOTTOM_SCREEN_MARGIN = 1.5f;
	protected const float PLAYER_X_MOVEMENT_BEFORE_CAM_FOLLOWS = .5f;
	protected const float CAM_X_SPEED_TO_FOLLOW = 10f;                   // how quickly camera catches up with its target movement in the x axis.
	protected const float CAM_Y_SPEED_TO_FOLLOW = 5f;                    // how quickly camera catches up with its target movement in the y axis.
	protected const float CAM_STARTING_X_POSITION = 0f;                  // starting position, from bottom left corner, in relation to player.
	protected const float CAM_STARTING_Y_POSITION = 0f;                  // starting position, from bottom left corner, in relation to player.

	// DEBUG FPS COUNTER
	protected const int FPS_ALIGNMENT = TOP_RIGHT;
	protected const float FPS_X_POS = 10f;
	protected const float FPS_Y_POS = 50f;

#endif

}