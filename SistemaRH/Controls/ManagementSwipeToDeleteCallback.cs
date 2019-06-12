using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using SistemaRH.Adapters;
using static SistemaRH.Enumerators.GlobalEnums;

namespace SistemaRH.Controls
{
    public class ManagementSwipeToDeleteCallback : ItemTouchHelper.SimpleCallback
    {
        private ManagementAdapter managementAdapter;
        private Drawable deleteIcon, acceptIcon;
        private readonly ColorDrawable deleteBackground, acceptBackground;
        private ManagementSwipeActions managementSwipeActions;

        public ManagementSwipeToDeleteCallback(ManagementAdapter managementAdapter, ManagementSwipeActions managementSwipeActions) : base(0, ItemTouchHelper.Start | ItemTouchHelper.End)
        {
            this.managementAdapter = managementAdapter;
            this.managementSwipeActions = managementSwipeActions;
            deleteIcon = ContextCompat.GetDrawable(Application.Context, Android.Resource.Drawable.IcMenuDelete);
            acceptIcon = ContextCompat.GetDrawable(Application.Context, Android.Resource.Drawable.IcMenuSave);
            deleteBackground = new ColorDrawable(Color.Red);
            acceptBackground = new ColorDrawable(new Color(ContextCompat.GetColor(Application.Context, Resource.Color.colorPrimary)));
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            int position = viewHolder.AdapterPosition;
            if (direction == ItemTouchHelper.Left && (managementSwipeActions == ManagementSwipeActions.Delete || managementSwipeActions == ManagementSwipeActions.DeleteAndAdd))
            {
                var item = managementAdapter.Items[position];
                managementAdapter.Items.Remove(item);
                managementAdapter.NotifyItemRemoved(position);
                managementAdapter.Fragment.ManagementOperationsListener?.RemoveItem(item).GetAwaiter();
            }
            else if (direction == ItemTouchHelper.Right && (managementSwipeActions == ManagementSwipeActions.Add || managementSwipeActions == ManagementSwipeActions.DeleteAndAdd))
            {
                var item = managementAdapter.Items[position];
                managementAdapter.Items.Remove(item);
                managementAdapter.NotifyItemRemoved(position);
                managementAdapter.Fragment.ManagementOperationsListener?.AddItem(item).GetAwaiter();
            }
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {          
            View itemView = viewHolder.ItemView;
            int backgroundCornerOffset = 20;
            int iconMargin = (itemView.Height - deleteIcon.IntrinsicHeight) / 2;
            int iconTop = itemView.Top + (itemView.Height - deleteIcon.IntrinsicHeight) / 2;
            int iconBottom = iconTop + deleteIcon.IntrinsicHeight;

            if (dX > 0 && (managementSwipeActions == ManagementSwipeActions.Add || managementSwipeActions == ManagementSwipeActions.DeleteAndAdd))
            { // Swiping to the right
                base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                int iconLeft = itemView.Left + iconMargin + acceptIcon.IntrinsicWidth;
                int iconRight = itemView.Left + iconMargin;
                acceptIcon.SetBounds(iconLeft, iconTop, iconRight, iconBottom);

                acceptBackground.SetBounds(itemView.Left, itemView.Top,
                        itemView.Left + ((int)dX) + backgroundCornerOffset,
                        itemView.Bottom);
                acceptBackground.Draw(c);
            }
            else if (dX < 0 && (managementSwipeActions == ManagementSwipeActions.Delete || managementSwipeActions == ManagementSwipeActions.DeleteAndAdd))
            { // Swiping to the left
                base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                int iconLeft = itemView.Right - iconMargin - deleteIcon.IntrinsicWidth;
                int iconRight = itemView.Right - iconMargin;
                deleteIcon.SetBounds(iconLeft, iconTop, iconRight, iconBottom);

                deleteBackground.SetBounds(itemView.Right + ((int)dX) - backgroundCornerOffset,
                        itemView.Top, itemView.Right, itemView.Bottom);
                deleteBackground.Draw(c);
            }
            else
            { // view is unSwiped
                deleteBackground.SetBounds(0, 0, 0, 0);
                acceptBackground.SetBounds(0, 0, 0, 0);
            } 
        }
    }
}