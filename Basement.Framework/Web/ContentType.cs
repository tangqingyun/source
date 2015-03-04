using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Basement.Framework.Utility.Web
{

    public enum ContentType
    {
        [Description("application/octet-stream")]
        stream,

        [Description("audio/x-mei-aac")]
        acp,

        [Description("application/postscript")]
        ai,

        [Description("audio/aiff")]
        aif,

        [Description("audio/aiff")]
        aifc,

        [Description("audio/aiff")]
        aiff,

        [Description("application/x-anv")]
        anv,

        [Description("text/asa")]
        asa,

        [Description("video/x-ms-asf")]
        asf,

        [Description("text/asp")]
        asp,

        [Description("video/x-ms-asf")]
        asx,

        [Description("audio/basic")]
        au,

        [Description("video/avi")]
        avi,

        [Description("application/vnd.adobe.workflow")]
        awf,

        [Description("text/xml")]
        biz,

        [Description("application/x-bmp")]
        bmp,

        [Description("application/x-bot")]
        bot,

        [Description("application/x-c4t")]
        c4t,

        [Description("application/x-c90")]
        c90,

        [Description("application/x-cals")]
        cal,

        [Description("application/vnd.ms-pki.seccat")]
        cat,

        [Description("application/x-netcdf")]
        cdf,

        [Description("application/x-cdr")]
        cdr,

        [Description("application/x-cel")]
        cel,

        [Description("application/x-x509-ca-cert")]
        cer,

        [Description("application/x-g4")]
        cg4,

        [Description("application/x-cgm")]
        cgm,

        [Description("application/x-cit")]
        cit,



        [Description("text/xml")]
        cml,

        [Description("application/x-cmp")]
        cmp,

        [Description("application/x-cmx")]
        cmx,

        [Description("application/x-cot")]
        cot,

        [Description("application/pkix-crl")]
        crl,

        [Description("application/x-x509-ca-cert")]
        crt,

        [Description("application/x-csi")]
        csi,

        [Description("text/css")]
        css,

        [Description("application/x-cut")]
        cut,

        [Description("application/x-dbf")]
        dbf,

        [Description("application/x-dbm")]
        dbm,

        [Description("application/x-dbx")]
        dbx,

        [Description("text/xml")]
        dcd,

        [Description("application/x-dcx")]
        dcx,

        [Description("application/x-x509-ca-cert")]
        der,

        [Description("application/x-dgn")]
        dgn,

        [Description("application/x-dib")]
        dib,

        [Description("application/x-msdownload")]
        dll,

        [Description("application/msword")]
        doc,

        [Description("application/msword")]
        dot,

        [Description("application/x-drw")]
        drw,

        [Description("text/xml")]
        dtd,

        [Description("Model/vnd.dwf")]
        dwf,

        [Description("application/x-dwg")]
        dwg,

        [Description("application/x-dxb")]
        dxb,

        [Description("application/x-dxf")]
        dxf,

        [Description("application/vnd.adobe.edn")]
        edn,

        [Description("application/x-emf")]
        emf,

        [Description("message/rfc822")]
        eml,

        [Description("text/xml")]
        ent,

        [Description("application/x-epi")]
        epi,

        [Description("application/postscript")]
        eps,

        [Description("application/x-ebx")]
        etd,

        [Description("application/x-msdownload")]
        exe,

        [Description("image/fax")]
        fax,

        [Description("application/vnd.fdf")]
        fdf,

        [Description("application/fractals")]
        fif,

        [Description("text/xml")]
        fo,

        [Description("application/x-frm")]
        frm,

        [Description("application/x-g4")]
        g4,

        [Description("application/x-gbr")]
        gbr,

        [Description("application/x-gcd")]
        gcd,

        [Description("image/gif")]
        gif,

        [Description("application/x-gl2")]
        gl2,

        [Description("application/x-gp4")]
        gp4,

        [Description("application/x-hgl")]
        hgl,

        [Description("application/x-hmr")]
        hmr,

        [Description("application/x-hpgl")]
        hpg,

        [Description("application/x-hpl")]
        hpl,

        [Description("application/mac-binhex40")]
        hqx,

        [Description("application/x-hrf")]
        hrf,

        [Description("application/hta")]
        hta,

        [Description("text/x-component")]
        htc,

        [Description("text/html")]
        htm,

        [Description("text/html")]
        html,

        [Description("text/webviewhtml")]
        htt,

        [Description("text/html")]
        htx,

        [Description("application/x-icb")]
        icb,

        [Description("image/x-icon")]
        ico,

        [Description("application/x-iff")]
        iff,

        [Description("application/x-g4")]
        ig4,

        [Description("application/x-igs")]
        igs,

        [Description("application/x-iphone")]
        iii,

        [Description("application/x-img")]
        img,

        [Description("application/x-internet-signup")]
        ins,

        [Description("application/x-internet-signup")]
        isp,

        [Description("video/x-ivf")]
        IVF,

        [Description("java/*")]
        java,

        [Description("image/jpeg")]
        jfif,

        [Description("image/jpeg")]
        jpe,

        [Description("image/jpeg")]
        jpeg,

        [Description("image/jpeg")]
        jpg,

        [Description("application/x-javascript")]
        js,

        [Description("text/html")]
        jsp,

        [Description("audio/x-liquid-file")]
        la1,

        [Description("application/x-laplayer-reg")]
        lar,

        [Description("application/x-latex")]
        latex,

        [Description("audio/x-liquid-secure")]
        lavs,

        [Description("application/x-lbm")]
        lbm,

        [Description("audio/x-la-lms")]
        lmsff,

        [Description("application/x-javascript")]
        ls,

        [Description("application/x-ltr")]
        ltr,

        [Description("video/x-mpeg")]
        m1v,

        [Description("video/x-mpeg")]
        m2v,

        [Description("audio/mpegurl")]
        m3u,

        [Description("video/mpeg4")]
        m4e,

        [Description("application/x-mac")]
        mac,

        [Description("application/x-troff-man")]
        man,

        [Description("text/xml")]
        math,

        [Description("application/msaccess")]
        mdb,

        [Description("application/x-shockwave-flash")]
        mfp,

        [Description("message/rfc822")]
        mht,

        [Description("message/rfc822")]
        mhtml,

        [Description("application/x-mi")]
        mi,

        [Description("audio/mid")]
        mid,

        [Description("audio/mid")]
        midi,

        [Description("application/x-mil")]
        mil,

        [Description("text/xml")]
        mml,

        [Description("audio/x-musicnet-download")]
        mnd,

        [Description("audio/x-musicnet-stream")]
        mns,

        [Description("application/x-javascript")]
        mocha,

        [Description("video/x-sgi-movie")]
        movie,

        [Description("audio/mp1")]
        mp1,

        [Description("audio/mp2")]
        mp2,

        [Description("video/mpeg")]
        mp2v,

        [Description("audio/mp3")]
        mp3,

        [Description("video/mpeg4")]
        mp4,

        [Description("video/x-mpg")]
        mpa,

        [Description("application/vnd.ms-project")]
        mpd,

        [Description("video/x-mpeg")]
        mpe,

        [Description("video/mpg")]
        mpeg,

        [Description("video/mpg")]
        mpg,

        [Description("audio/rn-mpeg")]
        mpga,

        [Description("application/vnd.ms-project")]
        mpp,

        [Description("video/x-mpeg")]
        mps,

        [Description("application/vnd.ms-project")]
        mpt,

        [Description("video/mpg")]
        mpv,

        [Description("video/mpeg")]
        mpv2,

        [Description("application/vnd.ms-project")]
        mpw,

        [Description("application/vnd.ms-project")]
        mpx,

        [Description("text/xml")]
        mtx,

        [Description("application/x-mmxp")]
        mxp,

        [Description("image/pnetvue")]
        net,

        [Description("application/x-nrf")]
        nrf,

        [Description("message/rfc822")]
        nws,

        [Description("text/x-ms-odc")]
        odc,


        [Description("application/pkcs10")]
        p10,

        [Description("application/x-pkcs12")]
        p12,

        [Description("application/x-pkcs7-certificates")]
        p7b,

        [Description("application/pkcs7-mime")]
        p7c,

        [Description("application/pkcs7-mime")]
        p7m,

        [Description("application/x-pkcs7-certreqresp")]
        p7r,

        [Description("application/pkcs7-signature")]
        p7s,

        [Description("application/x-pc5")]
        pc5,

        [Description("application/x-pci")]
        pci,

        [Description("application/x-pcl")]
        pcl,

        [Description("application/x-pcx")]
        pcx,

        [Description("application/pdf")]
        pdf,

        [Description("application/vnd.adobe.pdx")]
        pdx,

        [Description("application/x-pkcs12")]
        pfx,

        [Description("application/x-pgl")]
        pgl,

        [Description("application/x-pic")]
        pic,

        [Description("application/vnd.ms-pki.pko")]
        pko,

        [Description("application/x-perl")]
        pl,

        [Description("text/html")]
        plg,

        [Description("audio/scpls")]
        pls,

        [Description("application/x-plt")]
        plt,

        [Description("image/png")]
        png,

        [Description("application/vnd.ms-powerpoint")]
        pot,

        [Description("application/vnd.ms-powerpoint")]
        ppa,

        [Description("application/x-ppm")]
        ppm,

        [Description("application/vnd.ms-powerpoint")]
        pps,

        [Description("application/vnd.ms-powerpoint")]
        ppt,

        [Description("application/x-pr")]
        pr,

        [Description("application/pics-rules")]
        prf,

        [Description("application/x-prn")]
        prn,

        [Description("application/x-prt")]
        prt,

        [Description("application/x-ps")]
        ps,

        [Description("application/x-ptn")]
        ptn,

        [Description("application/vnd.ms-powerpoint")]
        pwz,

        [Description("text/vnd.rn-realtext3d")]
        r3t,

        [Description("audio/vnd.rn-realaudio")]
        ra,

        [Description("audio/x-pn-realaudio")]
        ram,

        [Description("application/x-ras")]
        ras,

        [Description("application/rat-file")]
        rat,

        [Description("text/xml")]
        rdf,

        [Description("application/vnd.rn-recording")]
        rec,

        [Description("application/x-red")]
        red,

        [Description("application/x-rgb")]
        rgb,

        [Description("application/vnd.rn-realsystem-rjs")]
        rjs,

        [Description("application/vnd.rn-realsystem-rjt")]
        rjt,

        [Description("application/x-rlc")]
        rlc,

        [Description("application/x-rle")]
        rle,

        [Description("application/vnd.rn-realmedia")]
        rm,

        [Description("application/vnd.adobe.rmf")]
        rmf,

        [Description("audio/mid")]
        rmi,

        [Description("application/vnd.rn-realsystem-rmj")]
        rmj,

        [Description("audio/x-pn-realaudio")]
        rmm,

        [Description("application/vnd.rn-rn_music_package")]
        rmp,

        [Description("application/vnd.rn-realmedia-secure")]
        rms,

        [Description("application/vnd.rn-realmedia-vbr")]
        rmvb,

        [Description("application/vnd.rn-realsystem-rmx")]
        rmx,

        [Description("application/vnd.rn-realplayer")]
        rnx,

        [Description("image/vnd.rn-realpix")]
        rp,

        [Description("audio/x-pn-realaudio-plugin")]
        rpm,

        [Description("application/vnd.rn-rsml")]
        rsml,

        [Description("text/vnd.rn-realtext")]
        rt,

        [Description("application/msword")]
        rtf,

        [Description("video/vnd.rn-realvideo")]
        rv,

        [Description("application/x-sam")]
        sam,

        [Description("application/x-sat")]
        sat,

        [Description("application/sdp")]
        sdp,

        [Description("application/x-sdw")]
        sdw,

        [Description("application/x-stuffit")]
        sit,

        [Description("application/x-slb")]
        slb,

        [Description("application/x-sld")]
        sld,

        [Description("drawing/x-slk")]
        slk,

        [Description("application/smil")]
        smi,

        [Description("application/smil")]
        smil,

        [Description("application/x-smk")]
        smk,

        [Description("audio/basic")]
        snd,

        [Description("text/plain")]
        sol,

        [Description("text/plain")]
        sor,

        [Description("application/x-pkcs7-certificates")]
        spc,

        [Description("application/futuresplash")]
        spl,

        [Description("text/xml")]
        spp,

        [Description("application/streamingmedia")]
        ssm,

        [Description("application/vnd.ms-pki.certstore")]
        sst,

        [Description("application/vnd.ms-pki.stl")]
        stl,

        [Description("text/html")]
        stm,

        [Description("application/x-sty")]
        sty,

        [Description("text/xml")]
        svg,

        [Description("application/x-shockwave-flash")]
        swf,

        [Description("application/x-tdf")]
        tdf,

        [Description("application/x-tg4")]
        tg4,

        [Description("application/x-tga")]
        tga,

        [Description("image/tiff")]
        tif,

        [Description("image/tiff")]
        tiff,

        [Description("text/xml")]
        tld,

        [Description("drawing/x-top")]
        top,

        [Description("application/x-bittorrent")]
        torrent,

        [Description("text/xml")]
        tsd,

        [Description("text/plain")]
        txt,

        [Description("application/x-icq")]
        uin,

        [Description("text/iuls")]
        uls,

        [Description("text/x-vcard")]
        vcf,

        [Description("application/x-vda")]
        vda,

        [Description("application/vnd.visio")]
        vdx,

        [Description("text/xml")]
        vml,

        [Description("application/x-vpeg005")]
        vpg,

        [Description("application/vnd.visio")]
        vsd,

        [Description("application/vnd.visio")]
        vss,

        [Description("application/vnd.visio")]
        vst,

        [Description("application/vnd.visio")]
        vsw,

        [Description("application/vnd.visio")]
        vsx,

        [Description("application/vnd.visio")]
        vtx,

        [Description("text/xml")]
        vxml,

        [Description("audio/wav")]
        wav,

        [Description("audio/x-ms-wax")]
        wax,

        [Description("application/x-wb1")]
        wb1,

        [Description("application/x-wb2")]
        wb2,

        [Description("application/x-wb3")]
        wb3,

        [Description("image/vnd.wap.wbmp")]
        wbmp,

        [Description("application/msword")]
        wiz,

        [Description("application/x-wk3")]
        wk3,

        [Description("application/x-wk4")]
        wk4,

        [Description("application/x-wkq")]
        wkq,

        [Description("application/x-wks")]
        wks,

        [Description("video/x-ms-wm")]
        wm,

        [Description("audio/x-ms-wma")]
        wma,

        [Description("application/x-ms-wmd")]
        wmd,

        [Description("application/x-wmf")]
        wmf,

        [Description("text/vnd.wap.wml")]
        wml,

        [Description("video/x-ms-wmv")]
        wmv,

        [Description("video/x-ms-wmx")]
        wmx,

        [Description("application/x-ms-wmz")]
        wmz,

        [Description("application/x-wp6")]
        wp6,

        [Description("application/x-wpd")]
        wpd,

        [Description("application/x-wpg")]
        wpg,

        [Description("application/vnd.ms-wpl")]
        wpl,

        [Description("application/x-wq1")]
        wq1,

        [Description("application/x-wr1")]
        wr1,

        [Description("application/x-wri")]
        wri,

        [Description("application/x-wrk")]
        wrk,

        [Description("application/x-ws")]
        ws,

        [Description("application/x-ws")]
        ws2,

        [Description("text/scriptlet")]
        wsc,

        [Description("text/xml")]
        wsdl,

        [Description("video/x-ms-wvx")]
        wvx,

        [Description("application/vnd.adobe.xdp")]
        xdp,

        [Description("text/xml")]
        xdr,

        [Description("application/vnd.adobe.xfd")]
        xfd,

        [Description("application/vnd.adobe.xfdf")]
        xfdf,

        [Description("text/html")]
        xhtml,

        [Description("application/x-xls")]
        xls,

        [Description("application/x-xlw")]
        xlw,

        [Description("text/xml")]
        xml,

        [Description("audio/scpls")]
        xpl,

        [Description("text/xml")]
        xq,

        [Description("text/xml")]
        xql,

        [Description("text/xml")]
        xquery,

        [Description("text/xml")]
        xsd,

        [Description("text/xml")]
        xsl,

        [Description("text/xml")]
        xslt,

        [Description("application/x-xwd")]
        xwd,

        [Description("application/x-x_b")]
        x_b,

        [Description("application/x-x_t")]
        x_t
    }  
}
