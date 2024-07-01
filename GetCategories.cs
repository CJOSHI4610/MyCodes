<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Code Analyzer</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css">
    <style>
        body {
            background-color: #121212;
            color: #e0e0e0;
            font-family: 'Courier New', Courier, monospace;
        }
        .container {
            margin-top: 50px;
        }
        .form-group label {
            color: #b0b0b0;
        }
        .btn-primary {
            background-color: #1f1f1f;
            border-color: #1f1f1f;
        }
        .btn-primary:hover {
            background-color: #373737;
            border-color: #373737;
        }
        .result, .result pre {
            margin-top: 20px;
            background-color: #1e1e1e;
            padding: 20px;
            border-radius: 5px;
            color: #00ff00; /* Green color for the results */
            font-weight: bold; /* Bold font for the results */
        }
        .hacker-header {
            text-align: center;
            margin-bottom: 50px;
        }
        .hacker-header h1 {
            font-size: 3rem;
            text-shadow: 2px 2px #000;
        }
        .hacker-header h2 {
            color: #00ff00;
        }
        .fa-terminal {
            font-size: 3rem;
            color: #00ff00;
        }
        .spinner {
            display: none;
            margin: 0 auto;
            text-align: center;
        }
        .spinner-border {
            color: #00ff00; /* Green color for the spinner */
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="hacker-header">
            <i class="fas fa-terminal"></i>
            <h1>Acumant Code Analyzer</h1>
            <h2>Hackathon Mode</h2>
        </div>
        <div>
            <h2>>> Search for a keyword in GitHub repository files</h2>
            <form id="search-form">
                <div class="form-group">
                    <label for="keyword">Keyword:</label>
                    <input type="text" class="form-control" id="keyword" required>
                </div>
                <button type="submit" class="btn btn-primary">Search</button>
            </form>
            <div class="spinner" id="search-spinner">
                <div class="spinner-border" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
            </div>
            <div class="result">
                <h3>Search Results:</h3>
                <pre id="search-result"></pre>
            </div>
        </div>
        <div>
            <h2>>> Fetch a file from GitHub repository and analyze</h2>
            <form id="analyze-form">
                <div class="form-group">
                    <label for="file_name">File Name:</label>
                    <input type="text" class="form-control" id="file_name" required>
                </div>
                <div class="form-group">
                    <label for="prompt_type">Analysis Type:</label>
                    <select class="form-control" id="prompt_type" required>
                        <option value="improvements">Code Review (PR) --> Suggest Improvements</option>
                        <option value="bugs_security">Analyze for Bugs and Security Vulnerabilities</option>
                        <option value="documentation">Generate Documentation</option>
                    </select>
                </div>
                <button type="submit" class="btn btn-primary">Analyze</button>
            </form>
            <div class="spinner" id="analyze-spinner">
                <div class="spinner-border" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
            </div>
            <div class="result">
                <h3>Analysis Result:</h3>
                <pre id="analyze-result"></pre>
            </div>
        </div>
        <div>
            <h2>>> Commit changes to GitHub</h2>
            <form id="commit-form">
                <div class="form-group">
                    <label for="commit_file_name">File Name:</label>
                    <input type="text" class="form-control" id="commit_file_name" required>
                </div>
                <div class="form-group">
                    <label for="updated_code">Updated Code:</label>
                    <textarea class="form-control" id="updated_code" rows="10" required></textarea>
                </div>
                <button type="submit" class="btn btn-primary">Commit</button>
            </form>
            <div class="spinner" id="commit-spinner">
                <div class="spinner-border" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
            </div>
            <div class="result">
                <h3>Commit Result:</h3>
                <pre id="commit-result"></pre>
            </div>
        </div>
    </div>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script>
        $(document).ready(function () {
            function showSpinner(id) {
                $(id).show();
            }

            function hideSpinner(id) {
                $(id).hide();
            }

            $('#search-form').on('submit', function (e) {
                e.preventDefault();
                showSpinner('#search-spinner');
                const keyword = $('#keyword').val();
                $.ajax({
                    type: 'POST',
                    url: '/search',
                    contentType: 'application/json',
                    data: JSON.stringify({ keyword: keyword }),
                    success: function (response) {
                        $('#search-result').html(response.results.join('<br>'));
                        hideSpinner('#search-spinner');
                    },
                    error: function (error) {
                        $('#search-result').text('Error: ' + error.responseJSON.error);
                        hideSpinner('#search-spinner');
                    }
                });
            });

            $('#analyze-form').on('submit', function (e) {
                e.preventDefault();
                showSpinner('#analyze-spinner');
                const fileName = $('#file_name').val();
                const promptType = $('#prompt_type').val();
                $.ajax({
                    type: 'POST',
                    url: '/analyze',
                    contentType: 'application/json',
                    data: JSON.stringify({ file_name: fileName, prompt_type: promptType }),
                    success: function (response) {
                        $('#analyze-result').html(response.result.replace(/\n/g, '<br>'));
                        hideSpinner('#analyze-spinner');
                    },
                    error: function (error) {
                        $('#analyze-result').text('Error: ' + error.responseJSON.error);
                        hideSpinner('#analyze-spinner');
                    }
                });
            });

            $('#commit-form').on('submit', function (e) {
                e.preventDefault();
                showSpinner('#commit-spinner');
                const fileName = $('#commit_file_name').val();
                const updatedCode = $('#updated_code').val();
                $.ajax({
                    type: 'POST',
                    url: '/commit',
                    contentType: 'application/json',
                    data: JSON.stringify({ file_name: fileName, updated_code: updatedCode }),
                    success: function (response) {
                        $('#commit-result').html(response.result.replace(/\n/g, '<br>'));
                        hideSpinner('#commit-spinner');
                    },
                    error: function (error) {
                        $('#commit-result').text('Error: ' + error.responseJSON.error);
                        hideSpinner('#commit-spinner');
                    }
                });
            });
        });
    </script>
</body>
</html>