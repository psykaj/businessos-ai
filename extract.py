import re
import os

with open("README.md", "r") as f:
    content = f.read()

# Split based on Day headings
parts = re.split(r'---*\n+## Day ', content)

header = parts[0]
day1 = "## Day " + parts[1].split('---')[0].strip()
day2 = "## Day " + parts[2].split('---')[0].strip()
day3 = "## Day " + parts[3].split('---')[0].strip()
tech_stack_and_running = "## " + parts[3].split('---')[1].strip() + "\n---\n" + "## " + parts[3].split('---')[2].strip()

# Create files
with open("docs/walkthroughs/day1-project-initialization.md", "w") as f:
    f.write(day1)

with open("docs/walkthroughs/day2-dashboard-ui.md", "w") as f:
    f.write(day2)

with open("docs/walkthroughs/day3-authentication.md", "w") as f:
    f.write(day3)

new_readme = header + """---

## 📚 Documentation Structure

The documentation for BusinessOS AI is organized into the `docs/` folder to ensure it remains scalable and easy to navigate.

- [**docs/README.md**](docs/README.md) - The main documentation index.
- [**docs/vision/**](docs/vision/) - Project vision and roadmap.
- [**docs/architecture/**](docs/architecture/) - Technical architecture and tech stack details.
- [**docs/plans/**](docs/plans/) - Detailed implementation plans and timelines.
- [**docs/walkthroughs/**](docs/walkthroughs/) - Step-by-step walkthroughs of completed work (Day 1, Day 2, etc.).
- [**docs/testing/**](docs/testing/) - QA and test reports.

---

""" + tech_stack_and_running

with open("README.md", "w") as f:
    f.write(new_readme)

print("Extraction complete.")
