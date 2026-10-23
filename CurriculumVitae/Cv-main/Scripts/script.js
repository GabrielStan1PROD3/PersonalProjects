



const btn = document.getElementById("btn");
const mobileLists = document.querySelector(".mobile-lists");
 const table = document.querySelector("table");
 const tds = document.querySelectorAll("td");
 const ths = document.querySelectorAll("th");

btn.addEventListener("click", () => {
  document.documentElement.classList.toggle("dark-mode");
  
  if (document.documentElement.classList.contains("dark-mode"))
    {
         mobileLists.style.boxShadow = "0 0 6px rgba(255,255,255,1)";
         mobileLists.style.border ="1px solid white";

        tds.forEach(td=>
            {
                
                td.style.boxShadow = "0 0 6px rgba(255,255,255,1)";
                td.style.border="1px solid white";
            });
             ths.forEach(th=>
            {
                th.style.boxShadow = "0 0 6px rgba(255,255,255,1)";
                th.style.border="1px solid white";
            })
    }
    else 
        {
                 mobileLists.style.boxShadow = "0 0 6px rgba(0, 0, 0, 1)";
                 mobileLists.style.border ="1px solid black";

        tds.forEach(td=>
            {
                
                td.style.boxShadow = "0 0 6px rgba(0, 0, 0, 1)";
                td.style.border="1px solid black";
            });
             ths.forEach(th=>
            {
                th.style.boxShadow = "0 0 6px rgba(0, 0, 0, 1)";
                th.style.border="1px solid black";;
            })
        }


});