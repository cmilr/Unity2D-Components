using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{

	// G L O B A L   S T A T I C
	// ~~~~~~~~~~~~~~~~~~~~~~~~~
	protected static bool debug_AttackDisabled;
	protected static bool debug_MovementDisabled;
	protected static bool debug_TileMapDisabled;


	// C O N S T A N T S
	// ~~~~~~~~~~~~~~~~~
	// global difficulty levels
	protected const int NORMAL 								= 1;
	protected const int HARD 								= 2;
	protected const int EPIC 								= 3;

	// global measurements
	protected const float ONE_PIXEL 						= .03125f;
	protected const float ONE_COLLIDER_PIXEL 				= .625f;
	protected const float ONE_SPRITE_HUD_PIXEL 				= 14.0625f;

	// global default culling distance
	protected const float CULL_DISTANCE 					= 20f;

	// gameObject names
	protected const string _DATA 							= "_Data";
	protected const string PLAYER 							= "Player";
	protected const string TILE_MAP 						= "TileMap";
	protected const string GAME_MANAGER 					= "GameManager";
	protected const string PICKUPS 							= "Pickups";

	// layer names
	protected const int PLAYER_DEFAULT_LAYER 				= 8;
	protected const int PLAYER_PHYSICS_LAYER 				= 8;
	protected const int PLAYER_BODY_COLLIDER 				= 10;
	protected const int PLAYER_WEAPON_COLLIDER 				= 9;
	protected const int ENEMY_BODY_COLLIDER 				= 14;
	protected const int ENEMY_WEAPON_COLLIDER 				= 17;
	protected const int BREAKABLES 							= 18;
	protected const int EDGE_BLOCKER 						= 24;
	protected const int PICKUP_LAYER 						= 15;
	protected const int PICKUP_PHYSICS_LAYER 				= 16;
	protected const int PLATFORM_LAYER 						= 21;
	protected const int NO_COLLISION_LAYER 					= 30;

	// z order
	protected const float PLAYER_Z 							= -.3f;
	protected const float IN_FRONT_OF_PLAYER_Z 				= -.31f;
	protected const float BEHIND_PLAYER_Z 					= -.29f;

	// sorting order
	protected const int FOREGROUND_ORDER 					= 500;
	protected const int ENEMY_PROJECTILE_ORDER 				= 310;
	protected const int PLAYER_ORDER 						= 300;
	protected const int PLAYER_WEAPON_ORDER 				= 290;
	protected const int PLAYER_PROJECTILE_ORDER 			= 280;
	protected const int ENEMY_ORDER 						= 200;
	protected const int ENEMY_WEAPON_ORDER 					= 190;
	protected const int PICKUP_ORDER 						= 100;
	protected const int BACKGROUND_ORDER 					= 0;
	protected const int SET_ME 								= 0;

	// pause while loading a level
	protected const float PAUSE_WPN_SWITCH_WHILE_LVL_LOADS	= 1.5f;
	protected const float PAUSE_ENEMIES_WHILE_LVL_LOADS 	= 3;


	// C A M E R A   &   H U D  
	// ~~~~~~~~~~~~~~~~~~~~~~~
	// CAMERA
	protected const float MIN_TOP_SCREEN_MARGIN 			= 4.25f;
	protected const float MIN_BOTTOM_SCREEN_MARGIN 			= 3f;
	protected const float PLYR_X_MOVE_BEFORE_CAM_FOLLOWS	= 0;
	protected const float CAM_X_SPEED_TO_FOLLOW 			= 20f;
	protected const float CAM_Y_SPEED_TO_FOLLOW 			= 5;
	protected const float CAM_STARTING_X_POSITION 			= 0;
	protected const float CAM_STARTING_Y_POSITION 			= 0;

	// HUD
	protected const float HUD_Z 							= 10f;
	protected const float HUD_FADE_IN_AFTER 				= .75f;
	protected const float HUD_FADE_OUT_AFTER 				= .25f;
	protected const float HUD_INITIAL_FADE_LENGTH 			= 2;


	// T E X T - B A S E D   H U D   E L E M E N T S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	// HUD ITEM TITLE
	protected const Position ITEM_TITLE_ALIGNMENT 			= Position.TopCenter;
	protected const float    ITEM_TITLE_X_POS 				= 0;
	protected const float    ITEM_TITLE_Y_POS 				= 1;

	// HUD XP
	protected const Position XP_ALIGNMENT 					= Position.TopLeft;
	protected const float    XP_X_POS 						= 0;
	protected const float    XP_Y_POS 						= 0;

	// HUD LEVEL
	protected const Position LEVEL_ALIGNMENT 				= Position.BottomLeft;
	protected const float    LEVEL_X_POS 					= 0;
	protected const float    LEVEL_Y_POS 					= 0;

	// HUD HP
	protected const Position HP_ALIGNMENT 					= Position.TopRight;
	protected const float    HP_X_POS 						= 0;
	protected const float    HP_Y_POS 						= 0;

	// HUD AC
	protected const Position AC_ALIGNMENT 					= Position.TopRight;
	protected const float    AC_X_POS 						= 0;
	protected const float    AC_Y_POS 						= 0;

	// HUD SCORE
	protected const Position SCORE_ALIGNMENT 				= Position.BottomCenter;
	protected const float    SCORE_X_POS 					= 0;
	protected const float 	 SCORE_Y_POS 					= .25f;


	// S P R I T E - B A S E D   H U D   E L E M E N T S
	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~å
	// HUD INVENTORY
	protected const Position INVENTORY_ALIGNMENT			= Position.TopCenter;
	protected const float    INVENTORY_X_POS 				=  0.0f;
	protected const float    INVENTORY_Y_POS 				=  -.25f;
	protected const float    STASHED_ITEM_OFFSET 			=  1.0f;
	protected const float    INVENTORY_SHIFT_SPEED 			=  0.15f;
	protected const float    ITEM_CHANGE_FADE 				=  0.0f;

	// HUD HEARTS
	protected const Position HEART_ALIGNMENT 				= Position.TopRight;
	protected const float    HEART_X_POS 					= -.15f;
	protected const float    HEART_Y_POS 					= -.15f;
	protected const float    HEART_OFFSET 					=  0.5f;

	// HUD WATER DROP
	protected const Position WATER_DROP_ALIGNMENT 			= Position.TopRight;
	protected const float    WATER_DROP_X_POS 				= -0.25f;
	protected const float    WATER_DROP_Y_POS 				= -1.75f;

	// DEBUG FPS COUNTER
	protected const Position FPS_ALIGNMENT 					= Position.TopRight;
	protected const float    FPS_X_POS 						=  0.0f;
	protected const float    FPS_Y_POS 						= -1.0f;
}