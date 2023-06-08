mergeInto(LibraryManager.library, {
    ShowAds: function () {
      ysdk.adv.showFullscreenAdv({
          callbacks: {
              onClose: function(wasShown) {
                MyGameInstance.SendMessage('Yandex', 'OnPlay');
              },
              onError: function(error) {
                MyGameInstance.SendMessage('Yandex', 'OnPlay');
              }
          }
      })
    },

});
