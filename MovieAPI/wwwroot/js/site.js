const baseUrl = "http://localhost:5000/api"
const functionalities = new Map();
functionalities.set("getMovie", `${baseUrl}/movies`);
functionalities.set("addMovie", `${baseUrl}/movies/add`);

function buildUrl(id) {
    let url = String();
    if (id == null) return functionalities.get("getMovie");
    else return `${functionalities.get("getMovie")}/${id}`;
}

async function getMovie(id=null)
{
    let url = buildUrl(id);
    const apiResponse = await fetch(url);
    if (!apiResponse.ok)
        throw new Error(`${apiResponse.status}: ${apiResponse.statusText}`);
    return await apiResponse.json()
        .then(data => data);
}

function addToTableOfMovies(movie) {
    const tableBody = document.querySelector("tbody");
    const tableRow = document.createElement("tr");
    const tableDataArray = [movie.title, movie.description, movie.author];
    for (let movieField = 0; movieField < tableDataArray.length; movieField++) {
        const thisTableData = document.createElement("td");
        thisTableData.textContent = tableDataArray[movieField];
        tableRow.appendChild(thisTableData);
    }
    tableBody.appendChild(tableRow);
}

(async function addMoviesToUI()
{
    const listOfMovies = await getMovie();
    for (const movie of listOfMovies) {
        addToTableOfMovies(movie);
    }
	const queryLastRow = getLastRowOfTheTable();
	if (!queryLastRow.found)
        return;
    const lastRow = queryLastRow.queryResult;
    lastRow.scrollIntoView();
})();

function getLastRowOfTheTable() {
  let queryResult = {};
  let lastRow;
  try {
  	lastRow = document.querySelector("tbody>:last-child");
    if (!lastRow) throw new Error('Empty table', 'No row found');
    queryResult["found"] = true;
	}
	catch (Error) {
  	queryResult["found"] = false;
  }
	finally {
    queryResult["queryResult"] = lastRow;
    return queryResult;
  }
}

const createMovieForm = document.querySelector("form");
createMovieForm.addEventListener("submit", 
    (e) => submitForm(e, this));

async function submitForm(e, form) {
    e.preventDefault();
    const submitBtn = document.querySelector("#submit-btn");

	submitBtn.disabled = true;
    setTimeout(() => submitBtn.disabled = false, 2000);
    const jsonFormData = buildJsonFormData(form);
    const headers = buildHeaders();
    const response = await performPostHttpRequest(functionalities.get("addMovie"), headers, jsonFormData);
    console.log(response);
    if (response === 201)
        window.location = "/index.html";
    else
        alert("Error");
}

function buildJsonFormData(form) {
    const movieFormFields = ["title", "description", "author"];
    const jsonFormData = { };
    for (const field of movieFormFields)
        jsonFormData[field] = document.querySelector(`#movie-${field}`).value;
    return jsonFormData;
}

function buildHeaders(authorization = null) {
    return {
        "Content-Type": "application/json",
        "Authorization": (authorization) ? authorization : "Bearer TOKEN_MISSING"
    };
}

async function performPostHttpRequest(fetchLink, headers, body) {
    if(!fetchLink || !headers || !body) {
        throw new Error("One or more POST request parameters was not passed.");
    }
    try {
        const rawResponse = await fetch(fetchLink, {
            method: "POST",
            headers: headers,
            body: JSON.stringify(body)
        });
        return rawResponse.status;
    }
    catch(err) {
        console.error(`Error at fetch POST: ${err}`);
        throw err;
    }
}
