/**
 * Copyright © 2018 Daniel Mester Pirttijärvi
 */
(function () {
    "use strict";
    
    function initMenu() {
        var expandables = document.querySelectorAll("a.expandable");
        var expandedLi;

        function clickOutsideHandler() {
            document.body.removeEventListener("click", clickOutsideHandler);

            if (expandedLi) {
                expandedLi.classList.remove("expanded");
                expandedLi = null;
            }
        }

        function getParentLi(el) {
            while (el && el.tagName !== "LI") {
                el = el.parentNode;
            }
            return el;
        }

        function clickHandler(ev) {
            var el = getParentLi(this);
            var previousExpandedLi = expandedLi;

            clickOutsideHandler();
            
            if (el && el !== previousExpandedLi) {
                document.body.addEventListener("click", clickOutsideHandler);
                ev.stopPropagation();
                el.classList.add("expanded");
                expandedLi = el;
            }
        }

        function hoverHandler(ev) {
            if (expandedLi) {
                var el = getParentLi(this);
                if (el && el !== expandedLi) {
                    expandedLi.classList.remove("expanded");
                    el.classList.add("expanded");
                    expandedLi = el;
                }
            }
        }

        for (var i = 0; i < expandables.length; i++) {
            var expandable = expandables[i];
            var span = document.createElement("span");

            while (expandable.firstChild) {
                span.appendChild(expandable.firstChild);
            }

            expandable.appendChild(span);
            expandable.addEventListener("click", clickHandler);
            expandable.addEventListener("mouseover", hoverHandler);
        }
    }

    initMenu();

})();