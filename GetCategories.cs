import subprocess

def run_build_check():
    try:
        # Run the dotnet build command
        result = subprocess.run(['dotnet', 'build'], capture_output=True, text=True, check=True)
        return result.stdout, None
    except subprocess.CalledProcessError as e:
        return None, e.stderr

if __name__ == "__main__":
    stdout, stderr = run_build_check()
    if stdout:
        print("Build succeeded:", stdout)
    if stderr:
        print("Build failed:", stderr)
