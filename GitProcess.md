# -----------------------------------------------------------------
# GIT Code review process (should be followed for all code modifications)
# -----------------------------------------------------------------

# Ensure that a GitHub issue has been created for the code that is being checked in
# Visit the issue page to retrieve issue number
# https://github.com/MacheteAdmin/Machete/issues

# Create a new branch for the current issue that you're working on:
# e.g. $ git checkout -b issues/56
(master) $ git checkout -b issues/{issue-number}

# Make changes, run tests, commit etc.
(issues/{issue-number}) $ subl {filename}
(issues/{issue-number}) $ git add .
(issues/{issue-number}) $ git commit

# Compare changes made from master branch
(master) $ git difftool master..issues/{issue-number}

# Push your commits to the remote for others to review:
(issues/{issue-number}) $ git push -u origin issues/{issue-number}

# Ask for feedback: Use the Github site to create a pull request

# After digesting your co-worker's comments, make further commits to your branch, push your new commits to the remote.
# GitHub will automatically update the pull request to contain your new changes, so you do not need to issue another pull request.
(issues/{issue-number}) $ subl {filename}
(issues/{issue-number}) $ git add .
(issues/{issue-number}) $ git commit
(issues/{issue-number}) $ git push

# This shows all commits on 'master' that aren't in your current branch history.
(issues/{issue-number}) $ git log ..master

# If you see any commits here (as a result of above command), then rebase the feature branch using following command.
# This replays your commits on top of the new commits from the destination branch so that the merge can be a 'fast-forward.
(issues/{issue-number}) $ git rebase master

# Merge feature work from local branch to master. 'merge --squash' squashes all the commits into 
# one commit, which is recommended because these commits most likely achieve the same goal. If you go 
# with "merge --squash", you need an extra "git commit" and modifying the commit message.

(issues/{issue-number}) $ git checkout master
(master) $ git merge --squash issues/{issue-number}
(master) $ git add .
(master) $ git commit

# Now delete the local and remote feature branches:
(master) $ git branch -D issues/{issue-number}
(master) $ git push origin :issues/{issue-number}

# Now that changes are in local-master branch, we need to commit and push to remote-master repo.
$ git add .
$ git commit
$ git push origin

# -----------------------------------------------------------------
# Useful GIT Articles
# -----------------------------------------------------------------

Github Pull requests (for code reviews) @
http://codeinthehole.com/writing/pull-requests-and-other-good-practices-for-teams-using-github/
http://wujingyue.blogspot.com/2012/12/pull-request-and-code-review-in-githubs.html

Git basic branching and merging tutorial @ 
http://git-scm.com/book/en/Git-Branching-Basic-Branching-and-Merging

Move file/directory from one repo to another repo @
http://gbayer.com/development/moving-files-from-one-git-repository-to-another-preserving-history/
http://stackoverflow.com/questions/1365541/how-to-move-files-from-one-git-repo-to-another-not-a-clone-preserving-history

# -----------------------------------------------------------------
# --Windows--: Useful Git configuration for suppressing password prompt for git-pull/push
# -----------------------------------------------------------------

# If and only if git-on-windows default-setup doesn't function correctly and unable
# to locate git keys then use this step:
# When using a custom filename for your keys (instead of the default id_rsa), then 
# 1) make sure to create this file: ~/.ssh/config: 
# 2) Add this text to the newly created file: IdentityFile ~/.ssh/{customname} e.g. IdentityFile ~/.ssh/github_rsa
# (replacing customname with the name of your key, without .pub)

# Open (or create if doesn't exist) ~/.bashrc file
notepad++ ~/.bashrc

# Put relevant git information into your prompt - this will make your life easier. 
# Here's a bash snippet for adding the current git branch to your prompt by adding it to ~/.bashrc file.

function parse_git_branch {
    git branch --no-color 2> /dev/null | sed -e '/^[^*]/d' -e 's/* \(.*\)/(\1) /'
}
PS1="\[\e[32m\]\$(parse_git_branch)\[\e[34m\]\h:\W \$ \[\e[m\]"
export PS1

# OBSOLETE: Add following lines to ~/.bashrc file: This was an old trick to have git 
# work with bash shell and not required anymore.

eval `ssh-agent`
ssh-add
openCode() { cd /d/Development/Projects/repositories/Test/splitbill ; }
openCode

# -----------------------------------------------------------------
# Useful Git configuration for setting default 'commit' editor
# -----------------------------------------------------------------

# --MacOSX--: Sets TextWrangler editor as default GIT commit editor.
# Make sure TextWrangler and its command-line tools are installed.
$ git config --global core.editor "edit -w"

# Another option is to make  Sublime default editor. You will have to
# to create symbolic-link for 'subl' to function from terminal:
$ ln -s "/Applications/Sublime Text.app/Contents/SharedSupport/bin/subl" /usr/local/bin/subl
$ git config --global core.editor "subl -n -w"

# --Windows--: Sets Notepad++ editor as default GIT commit editor.
# Make sure Notepad++ is installed.
$ git config --global core.editor "'c:/Program Files (x86)/Notepad++/notepad++.exe' -multiInst -notabbar -nosession -noPlugin"

# ------------------------------------------------------------------
# Useful Git configuration for difftool & diffmerge
#
# https://sourcegear.com/diffmerge/webhelp/sec__git__mac.html
# Reference website @ http://adventuresincoding.com/2010/04/how-to-setup-git-to-use-diffmerge

# -- Windows (pre-requisite)--: 
# 1) Install SourceGear DiffMerge (64-Bit) available for download from download.cnet.com
# 2) Make sure environment-variable PATH is appended with path to sgdm.exe
# -----------------------------------------------------------------

# To configure diff tool
$ git config --global diff.tool diffmerge

# --MacOsX--:
$ git config --global difftool.diffmerge.cmd "diffmerge \$LOCAL \$REMOTE"

# --Windows--:
# NOTE: if after setting below configuration, it still doesn't work then check
# the global-git-config file and makesure difftool cmd is configured with this 
# string: sgdm.exe \"$LOCAL\" \"$REMOTE\"
$ git config --global difftool.diffmerge.cmd "sgdm.exe \$LOCAL \$REMOTE"

# To launch difftool (that are not staged for commit yet)
$ git difftool

# To launch difftool for files that already staged for committed
git difftool --cached

# To configure merge tool
$ git config --global merge.tool diffmerge

# --MaxOsX--:
$ git config --global mergetool.diffmerge.cmd "diffmerge --merge --result=\$MERGED \$LOCAL \$BASE \$REMOTE"

# --Windows--:
$ git config --global mergetool.diffmerge.cmd "sgdm.exe --merge --result=\$MERGED \$LOCAL \$BASE \$REMOTE"

$ git config --global mergetool.diffmerge.trustExitCode true

# To launch mergetool
$ git mergetool

# -----------------------------------------------------------------
# Tell Git to ignore certain files
# -----------------------------------------------------------------
$ git config --global core.excludesfile {filepath e.g. => Test/common/gitconfigfiles/.gitignore}

# Following are some examples that have been tested on Windows successfully:
$ git config --global core.excludesfile common\gitconfigfiles\.gitignore
$ git config --global core.excludesfile "d:/Development/Projects/repositories/Test/common/gitconfigfiles/.gitignore"

# -----------------------------------------------------------------
# Setup useful GIT aliases
# -----------------------------------------------------------------

# Copy following git aliases to ~/.gitconfig file or alternatively you can set each alias 
# one by one with command like this: git config --global alias.co checkout

[alias]
    hist = log --color --pretty=format:\"%C(yellow)%h%C(reset) %s%C(bold red)%d%C(reset) %C(green)%ad%C(reset) %C(blue)[%an]%C(reset)\" --relative-date --decorate
    visualise = !gitk
    graph = log --color --graph --pretty=format:\"%h | %ad | %an | %s%d\" --date=short

# -----------------------------------------------------------------
# GIT: Setting your email address
# -----------------------------------------------------------------
Setting your email address for every repository on your computer:
	$ git config --global user.email "forshahzaib@gmail.com"

Setting your email address for a single repository: You may need to set a different email address for a single repository, such as a work email address for a work-related project.
	$ git config user.email "shahzaib@insidesocial.com"

# -----------------------------------------------------------------
# Useful GIT Commands
# -----------------------------------------------------------------

To pull repository first-time on local-machine
	$ git clone git@github.com:findingcue/Test.git

To see what are the pending changes (waiting to commit)
	$ git status

To see the history of check-in
	$ git log

Add pending changes (from local directory)
	$ git add .

To commit changes to local-repository
	$ git commit

To pull new changes made to repository
	$ git pull git@github.com:findingcue/Test.git

To push new changes to GITHUB server
	$ git push git@github.com:findingcue/Test.git

Do a dry run to see what will be added as a result of 'git add'
	$ git add -n .

To rename a file
	$ git mv {old-file-name} {new-file-name}

If above doesn't work then try the following sequence which is really what 'git mv' internally does
	$ mv {old-file-name} {new-file-name}
	$ git rm {old-file-name}
	$ git add {new-file-name}

To remove file from changes-not-staged-for-commit
	$ git checkout -- {filename}

To remove a file from changes-to-be-committed (cached index):
	$ git reset {file-name}

Show all git remote branches and the corresponding url-targets
	$ git remote -v

Show all git remote branches
	$ git remote show

Git remote show {remote-branch-name}
	$ git remote show origin
	$ git remote show upstream

Pulls in changes not present in your local repository, without modifying your files
	$ git fetch {branch-name} e.g. git fetch upstream

To stash current working changes, pull latest server changes, and then apply back the stashed changes
	$ git stash
	$ git pull origin
	$ git stash apply

