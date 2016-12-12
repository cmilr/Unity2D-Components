using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
	protected static bool debug_AttackDisabled;
	protected static bool debug_MovementDisabled;
	protected static bool debug_TileMapDisabled;

	// global difficulty levels
	protected const int NORMAL = 1;
	protected const int HARD = 2;
	protected const int EPIC = 3;

	// global measurements
	protected const float ONE_PIXEL = .03125f;
	protected const float ONE_COLLIDER_PIXEL = .625f;
    protected const float ONE_SPRITE_BASED_HUD_PIXEL = 14.0625f;

	// global default culling distance
	protected const float CULL_DISTANCE = 20f;

	// gameObject names
	protected const string _DATA = "_Data";
	protected const string PLAYER = "Player";
	protected const string TILE_MAP = "TileMap";
	protected const string GAME_MANAGER = "GameManager";
	protected const string PICKUPS = "Pickups";

	// layer names
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

	// sorting layer names
	protected const string PLATFORM_SORTING_LAYER = "Platforms";
	protected const string INTERACTIVE_SORTING_LAYER = "Interactives";
	protected const string PICKUP_SORTING_LAYER = "Pickups";
	protected const string ENEMY_SORTING_LAYER = "Enemies";
	protected const string HERO_ARM_SORTING_LAYER = "HeroArm";
	protected const string HERO_WEAPON_SORTING_LAYER = "HeroWeapon";
	protected const string HERO_SORTING_LAYER = "Hero";
	protected const string FOREGROUND_SORTING_LAYER = "Foreground";
	protected const string PROJECTILE_SORTING_LAYER = "Projectiles";

	// pause while loading a level
	protected const float PAUSE_WEAPON_SWITCHING_WHILE_LOADING_LEVEL = 1.5f;
	protected const float PAUSE_ENEMIES_WHILE_LOADING_LEVEL = 3f;


    // CAMERA & HUD
    // ~~~~~~~~~~~~

    // a couple global static variables for automatically scaling camera and HUD
    protected static float BASE_PIXELS_PER_UNIT;
	protected static float ORTHOGRAPHIC_FACTOR;
	protected static float CANVAS_SCALE;
	protected static float RESOLUTION_OFFSET;
    protected static float FINAL_ORTHOGRAPHIC_SIZE;


	// HUD
	protected const float HUD_Z = 10f;
	protected const float HUD_FADE_IN_AFTER = .75f;
	protected const float HUD_FADE_OUT_AFTER = .25f;
	protected const float HUD_INITIAL_TIME_TO_FADE = 2f;


	// TEXT-BASED HUD ELEMENTS
	// ~~~~~~~~~~~~~~~~~~~~~~~

	// HUD ITEM TITLE
	protected const Position ITEM_TITLE_ALIGNMENT = Position.TopCenter;
	protected const float ITEM_TITLE_X_POS = 0f;
	protected const float ITEM_TITLE_Y_POS = 20f;

	// HUD XP
	protected const Position XP_ALIGNMENT = Position.TopLeft;
	protected const float XP_X_POS = 140f;
	protected const float XP_Y_POS = 100f;

	// HUD LEVEL
	protected const Position LEVEL_ALIGNMENT = Position.TopLeft;
	protected const float LEVEL_X_POS = 123f;
	protected const float LEVEL_Y_POS = 320f;

	// HUD HP
	protected const Position HP_ALIGNMENT = Position.TopRight;
	protected const float HP_X_POS = 123f;
	protected const float HP_Y_POS = 320f;

	// HUD AC
	protected const Position AC_ALIGNMENT = Position.TopRight;
	protected const float AC_X_POS = 140f;
	protected const float AC_Y_POS = 200f;

	// HUD SCORE
	protected const Position SCORE_ALIGNMENT = Position.TopLeft;
	protected const float SCORE_X_POS = 123f;
	protected const float SCORE_Y_POS = 320f;


	// SPRITE-BASED HUD ELEMENTS
	// ~~~~~~~~~~~~~~~~~~~~~~~~~

	// HUD INVENTORY
	protected const float INVENTORY_Y_POS = 280f;
	protected const float STASHED_ITEM_OFFSET = 250f;
	protected const float DISTANCE_TO_SLIDE_ITEMS = 1.3281f;
	protected const float INVENTORY_SHIFT_SPEED = .15f;
	protected const float ITEM_CHANGE_FADE = 0f;

	// HUD HEARTS
	protected const Position HEART_ALIGNMENT = Position.TopRight;
	protected const float HEART_X_POS = 130f;
	protected const float HEART_Y_POS = 300f;
	protected const float HEART_OFFSET = 112.5f;

	// HUD WATER DROP
	protected const Position WATER_DROP_ALIGNMENT = Position.TopRight;
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
	protected const Position FPS_ALIGNMENT = Position.TopRight;
	protected const float FPS_X_POS = 10f;
	protected const float FPS_Y_POS = 50f;
}


// if in editor, adjust screen settings and add a fudge factor for sprite-based hud elements
//#if UNITY_EDITOR
//protected const float PLATFORM_SPECIFIC_ORTHOGRAPHIC_FACTOR = 112f;  // 32, 48, 64, 96, etc.
//protected const float PLATFORM_SPECIFIC_CANVAS_SCALE = .875f;
//protected const float IN_EDITOR_OFFSET = .50f;
//#endif

// otherwise, if in iOS build
//#if !UNITY_EDITOR
//protected const float PLATFORM_SPECIFIC_ORTHOGRAPHIC_FACTOR = 224f;
//protected const float PLATFORM_SPECIFIC_CANVAS_SCALE = 1.75f;
//protected const float IN_EDITOR_OFFSET = 1;
//#endif