function arkiveEmbedCallback(data) {
    var iframeCreation = '<iframe id="frame" name="widget" src ="#" width="100%" height="1" marginheight="0" marginwidth="0" frameborder="no"></iframe>';
    var iframe = window.location.protocol + "//" + (data.results[0].url);
    if (data.error != 'null') {
        document.getElementById("arkiveIframe").innerHTML = iframeCreation;
        var iframeAttr = parent.document.getElementById("frame");
        iframeAttr.height = arkiveApiHeight;
        iframeAttr.width = arkiveApiWidth + 22;
        iframeAttr.src = iframe;
    }
}
(function () {
    function async_load() {
        var s = document.createElement('script'); 
        s.type = 'text/javascript';
        s.async = true;
        s.src = 'https://api.arkive.org/v2/embedScript/species/scientificName/' + arkiveApiSpeciesName 
        + '?key=' + arkiveApiKey + (arkiveApiSpeciesId ? '&id=' + arkiveApiSpeciesId : '') 
        + '&mtype=all&w='
        + arkiveApiWidth + '&h=' + arkiveApiHeight + '&tn=' + (arkiveApiImages ? 1 : 0) + '&text=' 
        + (arkiveApiText ? 1 : 0) + '&callback=arkiveEmbedCallback';
        var x = document.getElementsByTagName('script')[0];
        x.parentNode.insertBefore(s, x);
    }
    if (window.attachEvent)
        window.attachEvent('onload', async_load);
    else
        window.addEventListener('load', async_load, false);
})();
