package com.yourcompany.filepicker;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.webkit.MimeTypeMap;
import com.unity3d.player.UnityPlayer;

public class FilePicker {
    private static final int FILE_SELECT_CODE = 0;
    private static UnityPlayerActivity activity;

    public static void pickFile() {
        activity = (UnityPlayerActivity) UnityPlayer.currentActivity;
        Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
        intent.setType("*/*");
        activity.startActivityForResult(Intent.createChooser(intent, "Select a file"), FILE_SELECT_CODE);
    }

    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == FILE_SELECT_CODE && resultCode == Activity.RESULT_OK) {
            Uri uri = data.getData();
            String path = FileUtils.getPath(activity, uri);
            UnityPlayer.UnitySendMessage("ProfileManager", "OnFilePicked", path);
        }
    }
}
