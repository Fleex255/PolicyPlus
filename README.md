# Policy Plus
Local Group Policy Editor plus more, for all Windows editions.

## Goals
Policy Plus is intended to make the power of Group Policy settings available to everyone.

* Run and work on all Windows editions, not just Pro and Enterprise
* Comply fully with licensing (i.e. transplant no components across Windows installations)
* View and edit Registry-based policies in local GPOs, per-user GPOs, individual POL files, offline Registry user hives, and the live Registry
* Navigate to policies by ID, text, or affected Registry entries
* Show additional technical information about objects (policies, categories, products)
* Provide convenient ways to share and import policy settings

Non-Registry-based policies (i.e. items outside the Administrative Templates branch of the Group Policy Editor) currently have no priority, 
but they may be reconsidered at a later date.

## Quick intro
At startup, Policy Plus opens the last saved policy source, or the local Group Policy Object (Local GPO) by default.
To open a different policy source (like a Registry branch or a per-user GPO), use *File | Open Policy Resources*.

Much like the official Group Policy editor, categories are shown in the left tree.
Information on the selected object is shown in the middle.
Policies and subcategories in the selected category are shown in the right list.
By default, both user and computer policies are displayed, but you can focus on just one policy source using the drop-down in the upper left.

To edit a policy, double-click it. If the selected setting applies to both users and computers,
you can switch sections with the "Editing for" drop-down. Click OK to keep the changes to the setting.
**Notice:** If a policy source is backed by a POL file (like Local GPO),
changes to it will not be committed to disk until you use *File | Save Policies* (Ctrl+S).

## Special considerations for use on Home editions
Some administrative templates are present by default on these editions, but many are missing. 
The newest full package can be downloaded from Microsoft and installed with *Help | Acquire ADMX Files*.

The `RefreshPolicyEx` native function does nothing on editions without full Group Policy infrastructure, so while Policy Plus can edit the local GPO and apply the changes to the Registry, 
a reboot or logon/logoff cycle is required for most policy changes to take effect.

While the UI allows the creation and editing of per-user GPOs, their settings are ignored by these limited editions of Windows. Edit per-user Registry hives instead.

## Status
Policy Plus is usable on all editions. It can load and save all policy sources successfully. More features may be still to come, though.

## Download
[Download a Release build.](https://s3-us-west-2.amazonaws.com/policy-plus/Policy%20Plus.exe)
This link will be updated for new commits after it is verified that they are fit for public use.
Note that Policy Plus is still pre-release software, so there may be bugs; please submit any problems to the issue tracker.