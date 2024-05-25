'use strict';

console.log("[+] Origin: User Script is running.");

let trigram;
let originalid;
let eventid;

// MutationObserver(callback) watches for changes being made in the DOM tree.
// When a change is made, the function callback is called.
const observer = new MutationObserver((mutationList) => {
    for (var mutation of mutationList){
        if (mutation.target.classList.toString().includes("expanded")){
            let applianceIndex;
            let tableHeader = document.querySelectorAll('table')[2].querySelector('tr').getElementsByTagName('th');
            console.log("[+] Class changed to Expanded detected")
            for (let i = 0; i < tableHeader.length; i++){
                if (tableHeader[i].getAttribute('aria-label') == "Appliance"){
                    applianceIndex = i;
                }
            }

            trigram = mutation.target.getElementsByTagName('td')[applianceIndex].innerText.split(":")[0];
            mutation.target.nextSibling.setAttribute('id', 'targeted'); // target the 'expanded-row' row that contain hidden information when not expanded  
            observer.disconnect(); // Stop monitoring but the MutationObserver instance is not destroyed
        }
    }
});

// Config (JavaScript Object Properties) used for the method MutationObserver.observe(target, config)
const config = {
    attributes: true,
    childList: true,
};

// Function called when a click event is recorded in the window.
function clickDetected(){
    let watchList = []; // List row with class == ""
    let watchExpandedList = []; // List row with class == "expanded-row"
    let eventid_elem;
    let originalid_elem;
    const rowList = document.querySelectorAll('table')[2].querySelector('tbody').querySelectorAll('tr'); 
    for (var i = 0; i < rowList.length; i++){
        if (!rowList[i].classList.toString() || rowList[i].classList.toString() == "highlight" || rowList[i].classList.toString() == "selected"){
            watchList.push(rowList[i]);
        }
        else if (rowList[i].classList.toString() == "expanded-row"){
            watchExpandedList.push(rowList[i]);
        }
    }

    for (let target of watchExpandedList){
        if (target.id == 'targeted'){ // targeted => Event row class change from "" to "expanded"
            try {
                for (i of target.querySelector('div.full-width').querySelectorAll("span.wrap-anywhere")){
                    if (i.innerText.includes('Original ID')){
                        originalid = i.innerText.split('\n').pop();
                        originalid_elem = i;
                    }
                    if (i.innerText.includes('Event ID')){
                        eventid = i.innerText.split('\n').pop();
                    }
                }
                originalid_elem.innerHTML = `<span class="t-label">Original ID<br><a href="https://localhost:1234/events?id=${originalid}" target="_blank" rel="noopener noreferrer">${originalid}</a></span>`;
                for (i of target.querySelectorAll('a.v-btn.v-btn--icon.v-btn--round.v-btn--router.theme--light.v-size--x-small.secondary.ml-2')){
                    if (i.pathname.includes("processtree")){
                        if (i.href.includes("xds")){
                            i.setAttribute("href", `https://localhost:1234/event/processtree?eventId=${originalid}`)
                            i.setAttribute("target", "_blank");
                            i.setAttribute("rel", "noopener noreferrer");
                        }     
                    }
                }
                console.log("[+] Event ID: " + eventid);
                console.log("[+] Original ID: " + originalid);
                target.removeAttribute('id'); // remove the mark to be recalled again after a click if the row is still expanded
            }
            catch (err){
                console.log("[!] Error detected");
                console.log(err)
            }
        }
    }

    // Monitore all the row with the class = ""
    for (let target of watchList){ 
        observer.observe(target, config);
    }

}

// Create event listener for click on the window
window.addEventListener("click", clickDetected);
